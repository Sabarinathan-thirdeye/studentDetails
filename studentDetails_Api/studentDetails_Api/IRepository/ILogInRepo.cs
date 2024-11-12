using studentDetails_Api.Models;
using studentDetails_Api.NonEntity;

namespace studentDetails_Api.IRepository
{
    public interface ILogInRepo
    {
        /// <summary>
        /// Addorupdate the StudentDetails 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<ApiResult<studentDetailModel>> AddOrUpdateStudentDetails(studentDetailModel student);
        /// <summary>
        /// Retrieves student details by email and validates password.
        /// </summary>
        Task<ApiResult<LogInResponseModel>> GetStudentByEmailAsync(LoginRequestModel req);
    }
}