using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Ynab.Dto;

namespace Ynab.Services
{
	public class YnabService : IYnabService
	{
		private const string Version = "v1";

		private readonly HttpClient _client;
		private readonly JsonSerializerOptions _serializerOptions;

		public YnabService(HttpClient client)
		{
			_client = client;
			_serializerOptions = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
		}

		public async Task<YnabResponse<ICollection<Budget>>> GetBudgetsAsync()
		{
			var apiResponse = await GetAsync(BuildUrl("budgets"));
			var response = new YnabResponse<ICollection<Budget>>
			{
				StatusCode = apiResponse.StatusCode
			};

			if (apiResponse.Success)
			{
				var prop = apiResponse.ResultObject.GetProperty("data").GetProperty("budgets");
				response.Value = JsonSerializer.Deserialize<List<Budget>>(prop.GetRawText(), _serializerOptions);
			}
			else
			{
				response.Error = JsonSerializer.Deserialize<Error>(apiResponse.ResultObject.GetProperty("error").GetRawText(), _serializerOptions);
			}

			return response;
		}

		public async Task<YnabResponse<ICollection<CategoryGroup>>> GetCategoriesAsync(string budgetId, bool includeHidden, bool includeDeleted)
		{
			var url = BuildUrl("budgets", budgetId, "categories");
			var apiResponse = await GetAsync(url);

			var response = new YnabResponse<ICollection<CategoryGroup>>
			{
				StatusCode = apiResponse.StatusCode
			};

			if (apiResponse.Success)
			{
				var prop = apiResponse.ResultObject.GetProperty("data").GetProperty("category_groups");
				response.Value = FilterCategories(JsonSerializer.Deserialize<ICollection<CategoryGroup>>(prop.GetRawText(), _serializerOptions), includeHidden, includeDeleted).ToList();
			}
			else
			{
				response.Error = JsonSerializer.Deserialize<Error>(apiResponse.ResultObject.GetProperty("error").GetRawText(), _serializerOptions);
			}

			return response;
		}

		public async Task<YnabResponse<Month>> GetMonthAsync(string budgetId, DateTime month)
		{
			var url = BuildUrl("budgets", budgetId, "months", month.ToString("yyyy-MM-01"));
			var apiResponse = await GetAsync(url);

			var response = new YnabResponse<Month>
			{
				StatusCode = apiResponse.StatusCode
			};

			if (apiResponse.Success)
			{
				var prop = apiResponse.ResultObject.GetProperty("data").GetProperty("month");
				response.Value = MapMonth(prop);
			}
			else
			{
				response.Error = JsonSerializer.Deserialize<Error>(apiResponse.ResultObject.GetProperty("error").GetRawText(), _serializerOptions);
			}

			return response;
		}

		public async Task<YnabResponse<ICollection<Transaction>>> GetTransactionsAsync(string budgetId, string categoryId, DateTime month)
		{
			var url = BuildUrl("budgets", budgetId, "categories", categoryId, "transactions");
			url += $"?since_date={month:yyyy-MM-01}";

			var apiResponse = await GetAsync(url);

			var response = new YnabResponse<ICollection<Transaction>>
			{
				StatusCode = apiResponse.StatusCode
			};

			if (apiResponse.Success)
			{
				var prop = apiResponse.ResultObject.GetProperty("data").GetProperty("transactions");
				response.Value = JsonSerializer.Deserialize<ICollection<Transaction>>(prop.GetRawText(), _serializerOptions);
			}
			else
			{
				response.Error = JsonSerializer.Deserialize<Error>(apiResponse.ResultObject.GetProperty("error").GetRawText(), _serializerOptions);
			}

			return response;
		}

		private async Task<(bool Success, JsonElement ResultObject, int StatusCode)> GetAsync(string path)
		{
			var response = await _client.GetAsync(path);
			var jsonObject = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
			var statusCode = (int)response.StatusCode;

			return (response.IsSuccessStatusCode, jsonObject, statusCode);
		}

		private string BuildUrl(params string[] parts)
		{
			return Version + "/" + string.Join('/', parts.Select(p => p.Trim('/')));
		}

		private IEnumerable<CategoryGroup> FilterCategories(IEnumerable<CategoryGroup> categories, bool includeHidden, bool includeDeleted)
		{
			IEnumerable<CategoryGroup> query = categories;

			if (!includeHidden)
				query = query.Where(e => !e.Hidden);

			if (!includeDeleted)
				query = query.Where(e => !e.Deleted);

			query = query.Select(g =>
			{
				var catQuery = g.Categories.AsEnumerable();

				if (!includeHidden)
					catQuery = catQuery.Where(e => !e.Hidden);

				if (!includeDeleted)
					catQuery = catQuery.Where(e => !e.Deleted);

				g.Categories = catQuery.ToList();
				return g;
			});

			return query;
		}

		private Month MapMonth(JsonElement element)
		{
			return new Month
			{
				Budgeted = element.GetProperty("budgeted").GetInt32(),
				Date = element.GetProperty("month").GetDateTime(),
				Income = element.GetProperty("income").GetInt32(),
				Deleted = element.GetProperty("deleted").GetBoolean(),
				Categories = element.GetProperty("categories")
				.EnumerateArray()
				.GroupBy(g => g.GetProperty("category_group_id").GetString())
				.Where(g => g.Count(e => !e.GetProperty("hidden").GetBoolean() && !e.GetProperty("deleted").GetBoolean()) > 0)
				.ToDictionary(kv => kv.Key, kv => kv.Where(e => !e.GetProperty("hidden").GetBoolean() && !e.GetProperty("deleted").GetBoolean())
				.Select(e => e.GetProperty("id").GetString()).ToList())
			};
		}
	}
}
