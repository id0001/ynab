using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Ynab;
using Ynab.Services;

namespace OsrsWeb.Vue.Controllers
{
	[ApiController]
	[Route("api/{controller}")]
	[Authorize]
	public class YnabController : ControllerBase
	{
		private readonly IYnabService _ynabService;

		public YnabController(IYnabService ynabService)
		{
			_ynabService = ynabService;
		}

		[Route("categories")]
		public async Task<IActionResult> GetCategoriesAsync(string budgetId, bool includeHidden = false, bool includeDeleted = false)
		{
			return CreateApiResponse(await _ynabService.GetCategoriesAsync(budgetId, includeHidden, includeDeleted));
		}

		[Route("budgets")]
		public async Task<IActionResult> GetBudgetsAsync()
		{
			return CreateApiResponse(await _ynabService.GetBudgetsAsync());
		}

		[Route("month")]
		public async Task<IActionResult> GetMonth(string budgetId, DateTime month)
		{
			return CreateApiResponse(await _ynabService.GetMonthAsync(budgetId, month));
		}

		[Route("transactions")]
		public async Task<IActionResult> GetTransactions(string budgetId, string categoryId, DateTime month)
		{
			return CreateApiResponse(await _ynabService.GetTransactionsAsync(budgetId, categoryId, month));
		}

		private IActionResult CreateApiResponse<T>(YnabResponse<T> response)
		{
			if (response.Success)
				return StatusCode(response.StatusCode, response.Value);

			return StatusCode(response.StatusCode, response.Error);
		}
	}
}
