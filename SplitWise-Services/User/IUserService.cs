namespace SplitWise_Services.User
{
    public interface IUserService
    {
        public SplitWise_Models.User createUser(string name);
        public SplitWise_Models.User GetUserByUserId(int id);
    }
}