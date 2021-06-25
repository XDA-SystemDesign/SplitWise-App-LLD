using SplitWise_Business.DTOs;
using SplitWise_Business.Exceptions;
using SplitWise_Models;
using SplitWise_Services.Group;
using SplitWise_Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Business.Validations
{
    public class GroupValidationService: IGroupValidationService
    {
        private IGroupService groupService;
        private IUserService userService;
        public GroupValidationService(IGroupService groupService, IUserService userService) => (this.groupService, this.userService) = (groupService, userService);

        public SplitWise_Models.Group createGroup(SplitWise_Models.Group group)
        {
            if (!ValidateGroup(group)) throw new ValidationException("Invalid Group entries");
            return this.groupService.createGroup(group);
        }
        public Group createGroupFromDTO(GroupDTO groupDTO)
        {
            
            if (groupDTO.Users.Count <= 1) throw new ValidationException("Please provide more number of users");
            Group group = new Group { name = groupDTO.GroupName, users = new List<User>()};
            foreach (var userName in groupDTO.Users)
            {
                User user = this.userService.createUser(userName.name);
                group.users.Add(user);
            }
            return this.groupService.createGroup(group);
        }
        public SplitWise_Models.Group GetGroupByGroupId(int id)
        {
            if (id <= 0) throw new ValidationException("Invalid ID");
            return groupService.GetGroupByGroupId(id);
        }
        private bool ValidateGroup(SplitWise_Models.Group group)
        {
            if (string.IsNullOrEmpty(group.name) || string.IsNullOrWhiteSpace(group.name)) return false;
            if (group.users.Count <= 1) return false;
            if (group.users.GroupBy(u => u.Userid).Count() > 1) return false;
            return true;
        }
    }
}
