using SplitWise_Business.DTOs;
using SplitWise_Models;

namespace SplitWise_Business.Validations
{
    public interface IGroupValidationService
    {
        public Group GetGroupByGroupId(int id);
        public Group createGroupFromDTO(GroupDTO groupDTO);
        public SplitWise_Models.Group createGroup(SplitWise_Models.Group group);
    }
}