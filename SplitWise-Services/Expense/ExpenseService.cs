using SplitWise_Models;
using SplitWise_Store.Expense;
using System.Collections.Generic;
using System.Linq;

namespace SplitWise_Services.Expense
{
    public class ExpenseService: IExpenseService
    {
        private IExpenseStore ExpenseStore;
        public ExpenseService(IExpenseStore Store) { 
            this.ExpenseStore = Store;
        }
        public SplitWise_Models.Expense CreateExpense(SplitWise_Models.Expense e, SplitWise_Models.Group g)
        {
            createContributionsIfNotPresent(g, ref e);
            return this.ExpenseStore.AddExpense(e);
        }

        public IEnumerable<ExpenseSummary> calculateSummaryForAnExpense(int ExpenseId)
        {
            var Expense = this.ExpenseStore.GetExpenseByExpenseId(ExpenseId);
            if (Expense is null) throw new System.Exception("Expense Id doesn't exists");
            return calculateSummaryForAnExpense(Expense);
        }

        public IEnumerable<SplitWise_Models.Expense> GetAllExpensesRelatedToUser(SplitWise_Models.User user)
        {
            return this.ExpenseStore.GetAllExpensesRelatedToUser(user);
        }
        private IList<ExpenseSummary> calculateSummaryForAnExpense(SplitWise_Models.Expense expense)
        {
            IList <ExpenseSummary> output = new List<ExpenseSummary>();
            foreach (var expenditure in expense.userExpenditures)
            {
                var userId = expenditure.UserId;
                if (output.Any(es => es.user.Userid == userId)) continue;
                var user = expense.group_associated_with_this.users.FirstOrDefault(user => user.Userid == userId);
                var his_contribution_towards_total = expense.userContributions.FirstOrDefault(uc => uc.UserId == userId).Contribution;
                output.Add(new ExpenseSummary { user = user, owe = his_contribution_towards_total, paid = expenditure.how_much_the_user_spent_on_the_group });
            }

            foreach (var contribution in expense.userContributions)
            {
                var userId = contribution.UserId;
                if (output.Any(es => es.user.Userid == userId)) continue;
                var user = expense.group_associated_with_this.users.FirstOrDefault(user => user.Userid == userId);
                var his_contribution_towards_total = contribution.Contribution;
                output.Add(new ExpenseSummary { user = user, owe = his_contribution_towards_total, paid = 0});
            }
            return output;
        }
        private void createContributionsIfNotPresent(SplitWise_Models.Group group, ref SplitWise_Models.Expense expenseRequest)
        {
            expenseRequest.group_associated_with_this = group;
            var total_money_spent_on_the_group = expenseRequest.userExpenditures.Sum(e => e.how_much_the_user_spent_on_the_group);
            if (expenseRequest.splitType == SplitWise_Constants.Enums.SplitType.EQUAL && !expenseRequest.userContributions.Any())
            {
                int number_of_users = group.users.Count;
                
                double each_contribution = total_money_spent_on_the_group / number_of_users;
                expenseRequest.userContributions = new();
                foreach (var user in group.users)
                {
                    expenseRequest.userContributions.Add(new UserContribution { Contribution = each_contribution, UserId = user.Userid });
                }
            } else if (expenseRequest.splitType == SplitWise_Constants.Enums.SplitType.PERCENTAGE)
            {
                foreach (var userContribution in expenseRequest.userContributions)
                {
                    userContribution.Contribution = (userContribution.Contribution * total_money_spent_on_the_group) / 100;
                }
            }
        }
    }
}
