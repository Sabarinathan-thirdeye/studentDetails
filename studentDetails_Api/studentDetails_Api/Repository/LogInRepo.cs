using studentDetails_Api.Models;
using studentDetails_Api.NonEntity;
using studentDetails_Api.IRepository;
using Microsoft.EntityFrameworkCore;
using studentDetails_Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace studentDetails_Api.Repository
{
    public class LogInRepo : ILogInRepo
    {
        private readonly StudentDBContext _context;
        /// <summary>
        /// Jwt Service class
        /// </summary>
        private readonly JwtServices _jwtServices;
        /// <summary>
        /// Current Http context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// Configuration
        /// </summary>
        private IConfiguration _configuration;
        /// <summary>
        /// Crypto Service class
        /// </summary>
        private readonly CryptoServices _cryptoServices;
        /// <summary>
        /// IUrlHelper
        /// </summary>
        private readonly IUrlHelper _urlHelper;

        /// <summary>
        /// Configuration
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jwtservices"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="configuration"></param>
        public LogInRepo(StudentDBContext context, JwtServices jwtservices, CryptoServices cryptoServices, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _jwtServices = jwtservices;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _cryptoServices = cryptoServices;
        }

        /// <summary>
        /// Adds or updates student details with encryption for sensitive fields.
        /// </summary>
        /// <param name="student">Student details to add or update.</param>
        /// <returns>ApiResult with the student details and operation status.</returns>
        public async Task<ApiResult<studentDetailModel>> AddOrUpdateStudentDetails(studentDetailModel student)
        {
            var result = new ApiResult<studentDetailModel>();
            try
            {
                var claimData = _httpContextAccessor.HttpContext?.Items["ClaimData"] as ClaimData;

                if (student == null)
                {
                    return result.ValidationErrorResponse("Please provide student details.");
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(student.studentName))
                {
                    return result.ValidationErrorResponse("Please provide first name.");
                }

                if (string.IsNullOrWhiteSpace(student.userName))
                {
                    return result.ValidationErrorResponse("Please provide last name.");
                }

                if (string.IsNullOrWhiteSpace(student.email))
                {
                    return result.ValidationErrorResponse("Please provide email.");
                }

                // Validate email format
                string emailRegexPattern = @"^[\w-]+(\.[\w-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,})$";
                if (!Regex.IsMatch(student.email, emailRegexPattern))
                {
                    return result.ValidationErrorResponse("Invalid email address.");
                }

                if (string.IsNullOrWhiteSpace(student.gender))
                {
                    return result.ValidationErrorResponse("Please provide gender.");
                }

                // Encrypt the password
                string encryptedPassword = _cryptoServices.EncryptStringToBytes_Aes(student.studentPassword);

                // Check for existing records
                var existingStudent = await _context.studentDetails
                    .FirstOrDefaultAsync(s => s.email == student.email && s.studentID != student.studentID);

                if (existingStudent != null)
                {
                    return result.ValidationErrorResponse("Email already exists.");
                }

                // Check if updating or creating a new record
                var studentRecord = await _context.studentDetails
                    .FirstOrDefaultAsync(s => s.studentID == student.studentID);

                if (studentRecord == null)
                {
                    // Add new student
                    studentRecord = new studentDetail
                    {
                        studentName = student.studentName,
                        userName = student.userName,
                        email = student.email,  // No encryption on email
                        mobileNumber = student.mobileNumber,
                        gender = student.gender,
                        dateOfBirth = student.dateOfBirth,
                        createdOn = DateTime.UtcNow,
                        createdBy = claimData?.UserID ?? 0,
                        studentPassword = encryptedPassword,  // Encrypt the password
                        studentstatus = student.studentstatus,
                    };

                    _context.studentDetails.Add(studentRecord);
                    await _context.SaveChangesAsync();

                    return result.SuccessResponse("Student created successfully.", student);
                }
                else
                {
                    // Update existing student
                    studentRecord.studentName = student.studentName;
                    studentRecord.userName = student.userName;
                    studentRecord.email = student.email;  // No encryption on email
                    studentRecord.mobileNumber = student.mobileNumber;
                    studentRecord.gender = student.gender;
                    studentRecord.dateOfBirth = student.dateOfBirth;
                    studentRecord.modifiedBy = claimData?.UserID ?? 0;
                    studentRecord.modifiedOn = DateTime.UtcNow;
                    studentRecord.studentPassword = encryptedPassword;  // Encrypt the password

                    await _context.SaveChangesAsync();

                    return result.SuccessResponse("Student updated successfully.", student);
                }
            }
            catch (Exception ex)
            {
                result.ResponseCode = -1;
                result.Message = "An error occurred while processing the request.";
                result.ErrorDesc = ex.Message;
                return result;
            }
        }


        /// <summary>
        /// Authenticate User
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResult<LogInResponseModel>> GetStudentByEmailAsync(LoginRequestModel login)
        {
            ApiResult<LogInResponseModel> result = new ApiResult<LogInResponseModel>();
            try
            {
                bool isTempPwdLogin = false;

                // Check if login data is null
                if (login == null)
                {
                    return result.ValidationErrorResponse("Please specify credentials");
                }

                // Check if email is provided
                if (string.IsNullOrEmpty(login.userName.Trim()))
                {
                    return result.ValidationErrorResponse("Please specify email");
                }

                // Check if password is provided
                if (string.IsNullOrEmpty(login.studentPassword.Trim()))
                {
                    return result.ValidationErrorResponse("Please specify password.");
                }

                // Retrieve the student record based on email
                var studentRecord = await _context.studentDetails.FirstOrDefaultAsync(u => u.userName == login.userName)
                ?? await _context.studentDetails.FirstOrDefaultAsync(u => u.email == login.userName);

                // Check if student exists
                if (studentRecord == null)
                {
                    return result.ValidationErrorResponse("No student record here");
                }

                // Decrypt the stored password
                string decryptedPassword = _cryptoServices.EncryptStringToBytes_Aes(login.studentPassword);

                // Compare decrypted password with entered password
                if (studentRecord.studentPassword != decryptedPassword)
                {
                    return result.ValidationErrorResponse("Invalid credentials.");
                }

                // Prepare the response model
                LogInResponseModel userInfo = new LogInResponseModel
                {
                    email = studentRecord.email,
                    userName =  studentRecord.userName,
                    dateOfBirth =  studentRecord.dateOfBirth,
                    mobileNumber =  studentRecord.mobileNumber,
                };

                // Generate JWT token
                userInfo.JwtToken = _jwtServices.GenerateToken(userInfo);

                // Return success response
                result.SuccessResponse("Login successful", userInfo);
                return result;
            }
            catch (Exception ex)
            {
                result.ResponseCode = -1;
                result.Message = "An error occurred while processing the request.";
                result.ErrorDesc = ex.Message;
                return result;
            }
        }
    }
}
