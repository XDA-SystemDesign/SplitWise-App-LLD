namespace SplitWise_Services.Group
{
    public interface IGroupService
    {
        SplitWise_Models.Group createGroup(SplitWise_Models.Group group);
        SplitWise_Models.Group GetGroupByGroupId(int groupId);
    }
}