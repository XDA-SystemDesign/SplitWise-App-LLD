using SplitWise_Constants.Enums;
using System.Collections.Generic;

namespace SplitWise_Models
{
    public class Expense
    {
        public int ExpenseId { get; set; }
        public Group group_associated_with_this { get; set; }
        public SplitType splitType { get; set; }
        public List<UserExpenditure> userExpenditures { get; set; }
        public List<UserContribution> userContributions { get; set; }
    }
    public class UserExpenditure
    {
        public int UserId { get; set; }
        public double how_much_the_user_spent_on_the_group { get; set; }
    }
    public class UserContribution
    {
        public int UserId { get; set; }
        public double Contribution { get; set; }
    }
    public class ExpenseSummary
    {
        public User user { get; set; }
        public double owe { get; set; }
        public double paid { get; set; }

        public override string ToString()
        {
            if ((int)paid == 0)
                return $"{user.name} owes {owe}";
            return $"{user.name} paid {paid} and owes {owe}";
        }
    }
}
