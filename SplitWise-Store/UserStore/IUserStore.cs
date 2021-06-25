using SplitWise_Models;

namespace SplitWise_Store.UserStore
{
    public interface IUserStore
    {
        public User createUser(string name);
        public User GetUserByUserId(int UserId);
    }
}