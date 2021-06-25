using SplitWise_Models;
using System.Collections.Generic;
using System.Linq;


namespace SplitWise_Store.Expense
{
    public class ExpenseBaseStore : IExpenseBaseStore
    {
        private IList<ExpenseBase> expenseBases;

        public ExpenseBaseStore() => this.expenseBases = new List<ExpenseBase>();
        public void addToExpenseStore(ExpenseBase Expenditure)
        {
            var existing_record = expenseBases.FirstOrDefault(e => e.Expense.ExpenseId == Expenditure.Expense.ExpenseId && e.User.Userid == e.User.Userid);
            if (existing_record is not null)
            {
                existing_record.Amount_paid += Expenditure.Amount_paid;
            } else
            {
                expenseBases.Add(Expenditure);
            }
        }
        public IEnumerable<ExpenseBase> GetExpenseBases(User user)
        {
            return expenseBases.Where(e => e.User.Userid == user.Userid);
        }
    }
}
