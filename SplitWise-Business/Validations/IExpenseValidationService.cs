using SplitWise_Business.DTOs;
using SplitWise_Models;
using System.Collections.Generic;

namespace SplitWise_Business.Validations
{
    public interface IExpenseValidationService
    {
        public Expense createExpense(ExpenseRequestDTO expenseRequest);
        public IEnumerable<ExpenseSummary> GetExpenseSummary(int id);
    }
}