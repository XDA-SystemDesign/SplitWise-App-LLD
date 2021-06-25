namespace SplitWise_Models
{
    public class Settlement
    {
        public Group Group_under_which_settlement_is_done { get; set; }
        public User FromUser { get; set; }
        public User ToUser { get; set; }
        public double Amount_Settled { get; set; }
    }
}
