namespace studentDetails_Api.NonEntity
{
    public class ClaimData
    {
        public long UserID { get; set; }
        public string UserName { get; set; } = null!;
        public long UserTypeID { get; set; }
        public string UserEmail { get; set; } = null!;
    }
}
