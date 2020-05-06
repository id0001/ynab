using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ynab;
using Ynab.Dto;
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
		public async Task<IActionResult> GetCategoriesAsync(string budgetId)
		{
			return CreateApiResponse(await _ynabService.GetCategoriesAsync(budgetId));
		}

		[Route("budgets")]
		public async Task<IActionResult> GetBudgetsAsync()
		{
			return CreateApiResponse(await _ynabService.GetBudgetsAsync());
		}

		private IActionResult CreateApiResponse<T>(YnabResponse<T> response)
		{
			if (response.Success)
				return StatusCode(response.StatusCode, response.Value);

			return StatusCode(response.StatusCode, response.Error);
		}
	}
}
