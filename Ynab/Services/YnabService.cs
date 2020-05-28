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
				response.Error = JsonSerializer.Deserialize<Error>(apiResponse.ResultObject.GetRawText(), _serializerOptions);
			}

			return response;
		}

		public async Task<YnabResponse<ICollection<CategoryGroup>>> GetCategoriesAsync(string budgetId)
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
				response.Value = JsonSerializer.Deserialize<ICollection<CategoryGroup>>(prop.GetRawText(), _serializerOptions);
			}
			else
			{
				response.Error = JsonSerializer.Deserialize<Error>(apiResponse.ResultObject.GetRawText(), _serializerOptions);
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
	}
}
