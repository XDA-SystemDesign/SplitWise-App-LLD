using SplitWise_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Store.GroupStore
{
    public class GroupStore: IGroupStore
    {
        private static int GroupId = 1;
        public IList<Group> groups;
        public GroupStore() => this.groups = new List<Group>();
        public Group createGroup(Group group)
        {
            group.GroupId = GroupId;
            this.groups.Add(group);
            GroupId++;
            return group;
        }
        public Group getGroupByGroupId(int groupId) => this.groups.FirstOrDefault(group => group.GroupId == groupId);
        public IEnumerable<SplitWise_Models.Group> getGroupsByUser(SplitWise_Models.User user) => this.groups.Where(group => group.users.Any(u => u == user));
    }
}
