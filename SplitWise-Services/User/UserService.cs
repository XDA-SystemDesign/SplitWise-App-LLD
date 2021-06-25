using SplitWise_Store.UserStore;
using SplitWise_Models;

namespace SplitWise_Services.User
{
    public class UserService: IUserService
    {
        private IUserStore userStore;
        public UserService(IUserStore userStore)
        {
            this.userStore = userStore;
        }
        public SplitWise_Models.User createUser(string name)
        {
            return this.userStore.createUser(name);
        }
        public SplitWise_Models.User GetUserByUserId(int userId) => this.userStore.GetUserByUserId(userId);
    }
}
