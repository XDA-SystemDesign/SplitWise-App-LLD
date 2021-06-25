using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Store.Expense
{
    public class ExpenseStore: IExpenseStore
    {
        private IList<SplitWise_Models.Expense> Expenses;
        private static int ExpenseId = 1;

        public ExpenseStore() => this.Expenses = new List<SplitWise_Models.Expense>();
        
        public SplitWise_Models.Expense AddExpense(SplitWise_Models.Expense e)
        {
            e.ExpenseId = ExpenseId;
            this.Expenses.Add(e);
            ExpenseId++;
            return e;
        }
        public SplitWise_Models.Expense GetExpenseByExpenseId(int ExpenseId) => this.Expenses.FirstOrDefault(e => e.ExpenseId == ExpenseId);

        public IEnumerable<SplitWise_Models.Expense> GetAllExpensesRelatedToUser(SplitWise_Models.User user)
        {
            return Expenses.Where(expense =>
            expense.userContributions.Any(u => u.UserId == user.Userid)
            || expense.userExpenditures.Any(u => u.UserId == user.Userid));
        }
    }
}
