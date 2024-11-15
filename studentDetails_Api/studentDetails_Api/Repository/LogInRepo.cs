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
        private readonly JwtServices _jwtServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly CryptoServices _cryptoServices;

        public LogInRepo(StudentDBContext context, JwtServices jwtservices, CryptoServices cryptoServices, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _jwtServices = jwtservices;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _cryptoServices = cryptoServices;
        }

        /// <summary>
        /// Registers a new student with encrypted sensitive fields.
        /// </summary>
        public async Task<ApiResult<studentDetailModel>> RegisterStudentDetail(studentDetailModel student)
        {
            var result = new ApiResult<studentDetailModel>();
            try
            {
                if (student == null)
                    return result.ValidationErrorResponse("Please provide student details.");

                // Validate required fields
                if (string.IsNullOrWhiteSpace(student.firstName))
                    return result.ValidationErrorResponse("Please provide the first name.");
                if (string.IsNullOrWhiteSpace(student.lastName))
                    return result.ValidationErrorResponse("Please provide the last name.");
                if (string.IsNullOrWhiteSpace(student.email))
                    return result.ValidationErrorResponse("Please provide an email.");

                string emailRegexPattern = @"^[\w-]+(\.[\w-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,})$";
                if (!Regex.IsMatch(student.email, emailRegexPattern))
                    return result.ValidationErrorResponse("Invalid email address format.");

                // Check if student already exists
                var existingStudent = await _context.studentDetails
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.email == student.email);

                if (existingStudent != null)
                    return result.ValidationErrorResponse("Email already exists.");

                // Add new student record
                var newStudent = new studentDetail
                {
                    firstName = student.firstName,
                    lastName = student.lastName,
                    email = student.email,
                    gender = student.gender,
                    dateOfBirth = student.dateOfBirth,
                    mobileNumber = student.mobileNumber,
                    studentstatus = student.studentstatus,
                    createdOn = DateTime.UtcNow,
                    createdBy = 1 // Replace with actual user ID from claims
                };

                _context.studentDetails.Add(newStudent);
                await _context.SaveChangesAsync();

                return result.SuccessResponse("Student registered successfully.", student);
            }
            catch (Exception ex)
            {
                result.ResponseCode = -1;
                result.Message = "An error occurred while registering the student.";
                result.ErrorDesc = ex.Message;
                return result;
            }
        }

        /// <summary>
        /// Registers a new user with encrypted sensitive fields.
        /// </summary>
        public async Task<ApiResult<userMasterModel>> RegisterUserDetail(userMasterModel user)
        {
            var result = new ApiResult<userMasterModel>();
            try
            {
                if (user == null)
                    return result.ValidationErrorResponse("Please provide user details.");

                // Validate required fields
                if (string.IsNullOrWhiteSpace(user.firstName))
                    return result.ValidationErrorResponse("Please provide the first name.");
                if (string.IsNullOrWhiteSpace(user.lastName))
                    return result.ValidationErrorResponse("Please provide the last name.");
                if (string.IsNullOrWhiteSpace(user.email))
                    return result.ValidationErrorResponse("Please provide an email.");

                string emailRegexPattern = @"^[\w-]+(\.[\w-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,})$";
                if (!Regex.IsMatch(user.email, emailRegexPattern))
                    return result.ValidationErrorResponse("Invalid email address format.");

                if (string.IsNullOrWhiteSpace(user.userPassword))
                    return result.ValidationErrorResponse("Please provide a password.");

                // Encrypt the password
                string encryptedPassword = _cryptoServices.EncryptStringToBytes_Aes(user.userPassword);

                // Check if user already exists
                var existingUser = await _context.userMasters.AsNoTracking() .FirstOrDefaultAsync(u => u.email == user.email);

                if (existingUser != null)
                    return result.ValidationErrorResponse("Email already exists.");

                // Add new user record
                var newUser = new userMaster
                {
                    firstName = user.firstName,
                    lastName = user.lastName,
                    userName = user.userName,
                    email = user.email,
                    userPassword = encryptedPassword,
                    dateOfBirth = user.dateOfBirth,
                    gender = user.gender,
                    createdOn = DateTime.UtcNow,
                    createdBy = 1, // Replace with actual user ID from claims
                    userTypeID = user.userTypeID,
                    userMasterStatus = user.userMasterStatus
                };

                _context.userMasters.Add(newUser);
                await _context.SaveChangesAsync();

                return result.SuccessResponse("User registered successfully.", user);
            }
            catch (Exception ex)
            {
                result.ResponseCode = -1;
                result.Message = "An error occurred while registering the user.";
                result.ErrorDesc = ex.InnerException?.Message ?? ex.Message; // Log the inner exception message
                return result;
            }

        }

        /// <summary>
        /// Authenticates a user by validating credentials and generating a JWT token.
        /// </summary>
        public async Task<ApiResult<LogInResponseModel>> Login(LoginRequestModel login)
        {
            var result = new ApiResult<LogInResponseModel>();
            try
            {
                if (login == null)
                    return result.ValidationErrorResponse("Please provide login credentials.");

                if (string.IsNullOrWhiteSpace(login.userName))
                    return result.ValidationErrorResponse("Please provide the username or email.");

                if (string.IsNullOrWhiteSpace(login.userPassword))
                    return result.ValidationErrorResponse("Please provide the password.");

                // Find user by username or email
                var user = await _context.userMasters
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.userName == login.userName || u.email == login.userName);

                if (user == null)
                    return result.ValidationErrorResponse("Invalid username or password.");

                // encrypt stored password for validation
                string encryptedPassword = _cryptoServices.EncryptStringToBytes_Aes(login.userPassword);

                if (login.userPassword == encryptedPassword)
                    return result.ValidationErrorResponse("Invalid password.");

                // Prepare the response model
                var response = new LogInResponseModel
                {
                    userName = user.userName,  // Mapping properties from userMaster
                    email = user.email,
                    mobileNumber = user.mobileNumber,
                    dateOfBirth = user.dateOfBirth,
                     // Generate JWT token
                };
                response.JwtToken = _jwtServices.GenerateToken(response);


                return result.SuccessResponse("Login successful.", response);  // Return the response
            }
            catch (Exception ex)
            {
                result.ResponseCode = -1;
                result.Message = "An error occurred while logging in.";
                result.ErrorDesc = ex.Message;
                return result;
            }
        }

    }
}
