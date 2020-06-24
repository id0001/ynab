using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ynab.Dto;

namespace Ynab.Services
{
	public interface IYnabService
	{
		Task<YnabResponse<ICollection<CategoryGroup>>> GetCategoriesAsync(string budgetId, bool includeHidden, bool includeDeleted);

		Task<YnabResponse<ICollection<Budget>>> GetBudgetsAsync();

		Task<YnabResponse<Month>> GetMonthAsync(string budgetId, DateTime month);

		Task<YnabResponse<ICollection<Transaction>>> GetTransactionsAsync(string budgetId, string categoryId, DateTime month);
	}
}
