using studentDetails_Api.Models;
using studentDetails_Api.NonEntity;

namespace studentDetails_Api.IRepository
{
    public interface ILogInRepo
    {
        /// <summary>
        /// Registers a new student's details.
        /// </summary>
        /// <param name="student">The student details model.</param>
        /// <returns>API result containing the registered student details.</returns>
        Task<ApiResult<studentDetailModel>> RegisterStudentDetail(studentDetailModel student);

        /// <summary>
        /// Registers a new user's details in the user master.
        /// </summary>
        /// <param name="user">The user details model.</param>
        /// <returns>API result containing the registered user details.</returns>
        Task<ApiResult<userMasterModel>> RegisterUserDetail(userMasterModel user);

        /// <summary>
        /// Authenticates a user by validating the email and password.
        /// </summary>
        /// <param name="req">The login request model containing username and password.</param>
        /// <returns>API result containing login response details (including JWT token) if successful.</returns>
        Task<ApiResult<LogInResponseModel>> Login(LoginRequestModel req);
    }
}
