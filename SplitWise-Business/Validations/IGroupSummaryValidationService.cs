using SplitWise_Business.DTOs;
using SplitWise_Models;

namespace SplitWise_Business.Validations
{
    public interface IGroupSummaryValidationService
    {
        public UserSummary getSummaryOfAUserInAGroup(GroupSummaryDTO groupSummary);
        public UserSummary getSummaryOfAUserInAGroup(int UserId, int GroupId);
    }
}