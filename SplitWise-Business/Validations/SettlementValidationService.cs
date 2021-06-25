using SplitWise_Business.DTOs;
using SplitWise_Business.Exceptions;
using SplitWise_Services.Group;
using SplitWise_Services.Settlement;
using SplitWise_Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Business.Validations
{
    public class SettlementValidationService: ISettlementValidationService
    {
        private ISettlementService settlementService;
        private IGroupService groupService;
        private IUserService userService;
        public SettlementValidationService(ISettlementService settlementService, IGroupService groupService, IUserService userService)
        {
            this.groupService = groupService;
            this.settlementService = settlementService;
            this.userService = userService;
        }

        public void createASettlementRecord(SettlementDTO settlement)
        {
            var (groupId, fromUserId, toUserId, amount_settled) = (settlement.GroupID_under_which_settlement_is_done,
                settlement.FromUserID, settlement.ToUserID, settlement.Amount_Settled);
            var (group, fromUser, toUser) = (groupService.GetGroupByGroupId(groupId), userService.GetUserByUserId(fromUserId), userService.GetUserByUserId(toUserId));
            if (groupId <= 0 || group is null) throw new ValidationException("invalid group sent");
            if (fromUserId <= 0 || fromUser is null) throw new ValidationException("invalid from user sent");
            if (toUserId <= 0 || toUser is null) throw new ValidationException("invalid to user sent");

            if (amount_settled < 0) throw new ValidationException("Invalid settling amount");
            // todo check if the amount_settled is less than the amount in the summary;
            this.settlementService.AddSettlement(new SplitWise_Models.Settlement { Amount_Settled = amount_settled, FromUser = fromUser, ToUser = toUser, Group_under_which_settlement_is_done = group });
        }
    }
}
