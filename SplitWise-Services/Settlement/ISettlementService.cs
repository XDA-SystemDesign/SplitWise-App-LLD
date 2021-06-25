using System.Collections.Generic;
namespace SplitWise_Services.Settlement
{
    public interface ISettlementService
    {
        public IEnumerable<SplitWise_Models.Settlement> GetAllSettlementsRelatedToUser(SplitWise_Models.User user, SplitWise_Models.Group group);
        void AddSettlement(SplitWise_Models.Settlement s);
    }
}