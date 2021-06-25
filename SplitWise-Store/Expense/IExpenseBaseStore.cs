using SplitWise_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Store.Expense
{
    public interface IExpenseBaseStore
    {
        
        public void addToExpenseStore(ExpenseBase Expenditure);
    }
}
