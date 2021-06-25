using SplitWise_Store.Settlements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Services.Settlement
{
    public class SettlementService: ISettlementService
    {
        private ISettlementStore store;
        public SettlementService(ISettlementStore store) => this.store = store;

        public void AddSettlement(SplitWise_Models.Settlement s)
        {
            this.store.addToSettlements(s);
        }
        public IEnumerable<SplitWise_Models.Settlement> GetAllSettlementsRelatedToUser(SplitWise_Models.User user, SplitWise_Models.Group group)
        {
            return this.store.getAllSettleMentsUnderTheGroup(group).Where(s => s.FromUser == user || s.ToUser == user);
        }
    }
}
