using SplitWise_Business.DTOs;
using SplitWise_Business.Exceptions;
using SplitWise_Models;
using SplitWise_Services.Calculation;
using SplitWise_Services.Group;
using SplitWise_Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Business.Validations
{
    public class GroupSummaryValidationService: IGroupSummaryValidationService
    {
        private ICalculationService calculationService;
        private IGroupService groupService;
        private IUserService userService;
        public GroupSummaryValidationService(ICalculationService calculationService, IGroupService groupService, IUserService userService)
        {
            (this.calculationService, this.groupService, this.userService) = (calculationService, groupService, userService);

        }

        public void validateInputs(int UserId, int GroupId)
        {
            if (UserId <= 0) throw new ValidationException("User Id is invalid");
            var user = this.userService.GetUserByUserId(UserId);
            if (user is null) throw new ValidationException("User Id is invalid");
            if (GroupId <= 0) throw new ValidationException("Group Id is invalid");
            var group = this.groupService.GetGroupByGroupId(GroupId);
            if (group is null) throw new ValidationException("Group Id is invalid");
            
        }
        public UserSummary getSummaryOfAUserInAGroup(GroupSummaryDTO groupSummary)
        {
            return getSummaryOfAUserInAGroup(groupSummary.UserId, groupSummary.GroupId);
        }

        public UserSummary getSummaryOfAUserInAGroup(int UserId, int GroupId)
        {
            try
            {
                validateInputs(UserId, GroupId);
                var user = this.userService.GetUserByUserId(UserId);
                var group = this.groupService.GetGroupByGroupId(GroupId);
                var output = this.calculationService.calculateSummaryForAGroup(group, user);
                if (output is null) return null;
                return output[user];
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}
