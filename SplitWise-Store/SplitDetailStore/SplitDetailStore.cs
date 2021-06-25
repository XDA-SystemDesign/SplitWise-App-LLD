using SplitWise_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SplitWise_Store.SplitDetailStore
{
    public class SplitDetailStore: ISplitDetailStore
    {
        private readonly IList<SplitDetail> splitDetails;
        public SplitDetailStore() => splitDetails = new List<SplitDetail>();
        //public void addToSplitDetail(SplitDetail s)
        //{
        //    var existing_record = splitDetails.FirstOrDefault(splitDetail => splitDetail.User == s.User && splitDetail.Expense.ExpenseId == s.Expense.ExpenseId);
        //    if (existing_record is not null) throw new Exception("A record under this expense is already present");
        //    splitDetails.Add(s);
        //}
    }
}
