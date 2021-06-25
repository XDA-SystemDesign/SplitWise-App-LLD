using SplitWise_Models;
using System.Linq;
using System.Collections.Generic;

namespace SplitWise_Store.UserStore
{
    public class UserStore: IUserStore
    {
        private List<User> users;
        private static int UserId = 1;

        public UserStore() => this.users = new List<User>();

        // For now only create. We can also go with deletion and update as well
        public User createUser(string name)
        {
            var user_object = new User(UserId, name);
            this.users.Add(user_object);
            UserId++;
            return user_object;
        }

        public User GetUserByUserId(int UserId) => this.users.FirstOrDefault(user => user.Userid == UserId);
    }
}
