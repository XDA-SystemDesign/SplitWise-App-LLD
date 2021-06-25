using SplitWise_Store.GroupStore;
using System.Collections.Generic;

namespace SplitWise_Services.Group
{
    public class GroupService: IGroupService
    {
        private IGroupStore groupStore;
        public GroupService(IGroupStore store) => this.groupStore = store;

        public SplitWise_Models.Group createGroup(SplitWise_Models.Group group)
        {
            return this.groupStore.createGroup(group);
        }

        public SplitWise_Models.Group GetGroupByGroupId(int groupId)
        {
            return groupStore.getGroupByGroupId(groupId);
        }

        public IEnumerable<SplitWise_Models.Group> GetGroupsByUser(SplitWise_Models.User user)
        {
            return groupStore.getGroupsByUser(user);
        }
    }
}