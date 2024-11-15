namespace studentDetails_Api.NonEntity
{
    /// <summary>
    /// Model for LogIn Inputs
    /// </summary>
    public  class LoginRequestModel
    {
        /// <summary>
        /// Email
        /// </summary>
        public string userName { get; set; } = null!;
        /// <summary>
        /// Student Password
        /// </summary>
        public string userPassword { get; set; } = null!;
        /// <summary>
        /// userName
        /// </summary>


    }

    /// <summary>
    /// Model for LogInResponse
    /// </summary>
    public class LogInResponseModel
    {
        public long studentID { get; set; }

        public string studentName { get; set; } = null!;

        public string userName { get; set; } = null!;

        public DateOnly dateOfBirth { get; set; }  

        public string email { get; set; } = null!;

        public long mobileNumber { get; set; }
   
        /// <summary>
        /// Auth Token
        /// </summary>
        public string JwtToken { get; set; } = null!;

        public string userPassword { get; set; } = null!;
    }
}
