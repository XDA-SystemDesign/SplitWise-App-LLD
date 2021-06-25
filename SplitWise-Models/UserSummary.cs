using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Models
{
    public class UserSummary
    {
        public User user { get; set; }
        public double amount { get; set; }
        public bool is_this_amount_to_be_paid_by_the_user() => amount <= 0 ? true : false;
        public List<Summary> summaries { get; set; }
        public override string ToString()
        {
            if (amount == 0) return "";
            StringBuilder s = new StringBuilder();
            if (is_this_amount_to_be_paid_by_the_user())
                s.AppendLine($"You owe {amount} overall");
            else s.AppendLine($"You're owed {amount} overall");
            foreach (var summary in summaries)
            {
                s.AppendLine($"You {summary.ToString()}");
            }
            return s.ToString();
        }
    }
    public class Summary
    {
        public User OtherUser { get; set; } // this will be from or to depending up on
        public double money_to_transfer { get; set; } // it'll be from or to depending up on the same
        public bool is_this_amount_to_be_paid_by_the_user() => money_to_transfer <= 0 ? true : false;
        public override string ToString()
        {
            if (money_to_transfer == 0) return "";
            if (is_this_amount_to_be_paid_by_the_user() && money_to_transfer < 0)
                return $" owe {OtherUser.name} -> {money_to_transfer}";
            else
                return $" are owed {money_to_transfer} from {OtherUser.name}";
        }
    }
}
