using studentDetails_Api.Models;
using studentDetails_Api.NonEntity;
using studentDetails_Api.IRepository;
using Microsoft.EntityFrameworkCore;

namespace studentDetails_Api.Repository
{
    public class LogInRepo : ILogInRepo
    {
        private readonly StudentDBContext _context;

        public LogInRepo(StudentDBContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<LogInResponseModel>> GetStudentByEmailAsync(LoginRequestModel req)
        {
            ApiResult<LogInResponseModel> result = new ApiResult<LogInResponseModel>();
            try
            {
                // Retrieve student by email
                var student = await _context.studentDetails
                    .FirstOrDefaultAsync(u => u.email == req.email && u.studentstatus != 99); // Ensure student is active

                if (student == null)
                {
                    result.ResponseCode = 0;
                    result.Message = "Student not found or deactivated.";
                    return result;
                }

                // Check password match (in real scenarios, hash the password)
                if (student.studentPassword != req.studentPassword) // You should use hashed passwords here
                {
                    result.ResponseCode = 0;
                    result.Message = "Invalid password.";
                    return result;
                }

                // Return login success with student details
                var loginResponse = new LogInResponseModel
                {
                    studentID = student.studentID,
                    firstName = student.firstName,
                    lastName = student.lastName,
                    email = student.email
                };

                result.ResponseCode = 1;
                result.Message = "Login successful.";
                result.Data = loginResponse;

                return result;
            }
            catch (Exception ex)
            {
                result.ResponseCode = -1;
                result.Message = "Error occurred during login.";
                result.ErrorDesc = ex.Message;
                return result;
            }
        }
    }
}
