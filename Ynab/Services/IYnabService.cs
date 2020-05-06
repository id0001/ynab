using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ynab.Dto;

namespace Ynab.Services
{
	public interface IYnabService
	{
		Task<YnabResponse<ICollection<CategoryGroup>>> GetCategoriesAsync(string budgetId);

		Task<YnabResponse<ICollection<Budget>>> GetBudgetsAsync();
	}
}
