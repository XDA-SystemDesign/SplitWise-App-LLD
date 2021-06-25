using SplitWise_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Store.Settlements
{
    public interface ISettlementStore
    {
        public void addToSettlements(Settlement s);
        public IEnumerable<Settlement> getAllSettleMentsUnderTheGroup(Group group);
    }
}
