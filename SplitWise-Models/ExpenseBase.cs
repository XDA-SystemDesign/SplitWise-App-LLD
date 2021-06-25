using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Models
{
    public class ExpenseBase
    {
        public Expense Expense { get; set; }
        public User User { get; set; }
        public float Amount_paid { get; set; }
    }
}
