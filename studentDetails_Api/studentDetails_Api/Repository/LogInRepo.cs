using studentDetails_Api.Models;
using studentDetails_Api.NonEntity;
using studentDetails_Api.IRepository;
using Microsoft.EntityFrameworkCore;
using studentDetails_Api.Services;

namespace studentDetails_Api.Repository
{
    public class LogInRepo : ILogInRepo
    {
        private readonly StudentDBContext _context;
        private readonly JwtServices _jwtServices;
        private readonly IHttpContextAccessor httpContextAccessor;
        private IConfiguration _config;

        public LogInRepo(StudentDBContext context, JwtServices jwtServices, IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _context = context;
            _jwtServices = jwtServices;
            this.httpContextAccessor = httpContextAccessor;
            _config = config;
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

                // Decrypt stored password
                var cryptoService = new CryptoServices(_config, this.httpContextAccessor);
                string decryptedPassword = cryptoService.DecryptStringFromBytes_Aes(student.studentPassword);

                // Compare decrypted password with the request password
                if (decryptedPassword != req.studentPassword)
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

                // Generate JWT token using the LogInResponseModel
                string jwtToken = _jwtServices.GenerateToken(loginResponse);
                loginResponse.JwtToken = jwtToken;

                // Set the response values
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
