
//using studentDetails_Api.Models;
//using System.ComponentModel.DataAnnotations;

//namespace studentDetails_Api.Common.NonEntities
//{
//    /// <summary>
//    /// Model for login inputs.
//    /// </summary>
//    //public class LogInModel
//    //{
//        /// <summary>
//        /// Gets or sets the user's email address.
//        /// </summary>
//        ///// <remarks>
//        ///// This field is required for logging in.
//        ///// </remarks>
//        //[Required(ErrorMessage = "Email is required")]
//        //public required string UserName { get; set; }

//        ///// <summary>
//        ///// Gets or sets the user's password.
//        ///// </summary>
//        ///// <remarks>
//        ///// This field is required for logging in.
//        ///// </remarks>
//        //[Required(ErrorMessage = "User Password is required.")]
//        //public required string UserPassword { get; set; }
//    //}

//    /// <summary>
//    /// Model for the login response details.
//    /// </summary>
//    public class LogInModel
//    {
//        ///// <summary>
//        ///// Gets or sets the unique identifier for the user.
//        ///// </summary>
//        //public int studentID { get; set; }

//        ///// <summary>
//        ///// Gets or sets the first name of the user.
//        ///// </summary>
//        //public string firstName { get; set; } = null!;

//        ///// <summary>
//        ///// Gets or sets the last name of the user.
//        ///// </summary>
//        //public string? lastName { get; set; }
//        ///// <summary>
//        ///// Username
//        ///// </summary>

//        //public DateOnly dateOfBirth { get; set; }
//        ///// <summary>
//        ///// Gets or sets the user's email address.
//        ///// </summary>
//        ///// 
//        //public string email { get; set; } = null!;
//        ///// <summary>
//        ///// Temporary password
//        ///// </summary>
//        ///// <summary>
//        ///// Gets or Sets the User logged in using Temp Password
//        ///// </summary>
//        //public long gender { get; set; }
//        ///// <summary>
//        ///// Gets or sets the user's phone number.
//        ///// </summary>
//        ///// <remarks>
//        ///// This field is optional and may be null.
//        ///// </remarks>
//        //public long mobileNumber { get; set; }
//        ///// <summary>
//        ///// Gets or sets the plan subscription
//        ///// </summary>
//        //public string studentPassword { get; set; } = null!;
//        ///// <summary>
//        ///// Gets or sets the authentication token for the user.
//        ///// </summary>
//        ///// <remarks>
//        ///// This token is used for authorizing the user in subsequent requests.
//        ///// </remarks>
//        //public string Token { get; set; } = null!;
//        [Required]
//        [EmailAddress]
//        public string Email { get; set; }

//        [Required]
//        [DataType(DataType.Password)]
//        public string Password { get; set; }
//    }
//    public class LogInResponse
//    {
//        public string Token { get; set; }
//        public string Message { get; set; }
//    }

//}

