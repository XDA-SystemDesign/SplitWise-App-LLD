using SplitWise_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWise_Store.GroupStore
{
    public interface IGroupStore
    {
        public Group createGroup(Group group);
        //public void addToGroup(int groupId, List<User> users);
        public Group getGroupByGroupId(int groupId);
        public IEnumerable<SplitWise_Models.Group> getGroupsByUser(SplitWise_Models.User user);
    }
}
