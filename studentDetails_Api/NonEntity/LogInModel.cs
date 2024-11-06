
//using System.ComponentModel.DataAnnotations;

//namespace studentDetails_Api.Common.NonEntities
//{
//    /// <summary>
//    /// Model for login inputs.
//    /// </summary>
//    public class LogInModel
//    {
//        /// <summary>
//        /// Gets or sets the user's email address.
//        /// </summary>
//        /// <remarks>
//        /// This field is required for logging in.
//        /// </remarks>
//        [Required(ErrorMessage = "Email is required")]
//        public required string UserName { get; set; }

//        /// <summary>
//        /// Gets or sets the user's password.
//        /// </summary>
//        /// <remarks>
//        /// This field is required for logging in.
//        /// </remarks>
//        [Required(ErrorMessage = "User Password is required.")]
//        public required string UserPassword { get; set; }
//    }

//    /// <summary>
//    /// Model for the login response details.
//    /// </summary>
//    public class LogInResponse
//    {
//        /// <summary>
//        /// Gets or sets the unique identifier for the user.
//        /// </summary>
//        public long UserID { get; set; }

//        /// <summary>
//        /// Gets or sets the identifier for the user's role or type.
//        /// </summary>
//        /// <remarks>
//        /// This corresponds to the UserTypeID in the UserType table.
//        /// </remarks>
//        public long UserTypeID { get; set; }

//        /// <summary>
//        /// Gets or sets the first name of the user.
//        /// </summary>
//        public string FirstName { get; set; } = null!;

//        /// <summary>
//        /// Gets or sets the last name of the user.
//        /// </summary>
//        public string? LastName { get; set; }
//        /// <summary>
//        /// Username
//        /// </summary>

//        public string Username { get; set; } = null!;
//        /// <summary>
//        /// Gets or sets the user's email address.
//        /// </summary>
//        public string UserEmail { get; set; } = null!;
//        /// <summary>
//        /// Temporary password
//        /// </summary>
//        /// <summary>
//        /// Gets or Sets the User logged in using Temp Password
//        /// </summary>
//        public bool IsTempPasswordLogin { get; set; }
//        /// <summary>
//        /// Gets or sets the user's phone number.
//        /// </summary>
//        /// <remarks>
//        /// This field is optional and may be null.
//        /// </remarks>
//        public string? Phonenumber { get; set; }
//        /// <summary>
//        /// Gets or sets the plan subscription
//        /// </summary>
//        public bool IsSubscribed { get; set; } // Added to indicate subscription status
//        /// <summary>
//        /// Gets or sets the authentication token for the user.
//        /// </summary>
//        /// <remarks>
//        /// This token is used for authorizing the user in subsequent requests.
//        /// </remarks>
//        public string Token { get; set; } = null!;
//    }   
//}

