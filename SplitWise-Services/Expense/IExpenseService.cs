using System.Collections.Generic;

namespace SplitWise_Services.Expense
{
    public interface IExpenseService
    {
        public SplitWise_Models.Expense CreateExpense(SplitWise_Models.Expense e, SplitWise_Models.Group g);
        public IEnumerable<SplitWise_Models.Expense> GetAllExpensesRelatedToUser(SplitWise_Models.User user);
        public IEnumerable<SplitWise_Models.ExpenseSummary> calculateSummaryForAnExpense(int ExpenseId);
    }
}