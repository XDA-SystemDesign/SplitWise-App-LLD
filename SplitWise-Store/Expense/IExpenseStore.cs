using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Store.Expense
{
    public interface IExpenseStore
    {
        public SplitWise_Models.Expense AddExpense(SplitWise_Models.Expense e);
        public SplitWise_Models.Expense GetExpenseByExpenseId(int ExpenseId);
        public IEnumerable<SplitWise_Models.Expense> GetAllExpensesRelatedToUser(SplitWise_Models.User user);
    }
}
