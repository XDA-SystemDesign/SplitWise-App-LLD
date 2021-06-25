using SplitWise_Models;
using System.Collections.Generic;
using System.Linq;

namespace SplitWise_Store.Settlements
{
    public class SettlementStore: ISettlementStore
    {
        private IList<Settlement> Settlements;
        public SettlementStore() => this.Settlements = new List<Settlement>();
        public void addToSettlements(Settlement s)
        {
            var existing_settlement = this.Settlements.FirstOrDefault(settlement => 
            settlement.Group_under_which_settlement_is_done.GroupId == s.Group_under_which_settlement_is_done.GroupId
            && settlement.FromUser == s.FromUser && settlement.ToUser == s.ToUser);
            if (existing_settlement is not null)
            {
                existing_settlement.Amount_Settled += s.Amount_Settled;
            }
            else Settlements.Add(s);
        }

        public IEnumerable<Settlement> getAllSettleMentsUnderTheGroup(Group group) 
            => this.Settlements.Where(s => s.Group_under_which_settlement_is_done.GroupId == group.GroupId);
    }
}
