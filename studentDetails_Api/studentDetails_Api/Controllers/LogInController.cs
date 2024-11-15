using Microsoft.AspNetCore.Mvc;
using studentDetails_Api.IRepository;
using studentDetails_Api.Models;
using studentDetails_Api.NonEntity;
using studentDetails_Api.Services;
using System;

namespace studentDetails_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        private readonly ILogInRepo _logInRepo;
        private readonly JwtServices _jwtServices;
        private readonly ILogger<LogInController> _logger;

        public LogInController(ILogInRepo logInRepo, JwtServices jwtServices, ILogger<LogInController> logger)
        {
            _logInRepo = logInRepo;
            _jwtServices = jwtServices;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="student">Student details for registration.</param>
        /// <returns>API result with registration status.</returns>
        [HttpPost("RegisterStudentDetail")]
        public async Task<IActionResult> RegisterStudentDetail([FromBody] studentDetailModel student)
        {
            ApiResult<studentDetailModel> result = new ApiResult<studentDetailModel>();
            try
            {
                result = await _logInRepo.RegisterStudentDetail(student);

                return result.ResponseCode == 1
                    ? Ok(result)
                    : StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while registering student details.");
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while registering student details.", ex));
            }
        }

        /// <summary>
        /// Registers a new user in the user master.
        /// </summary>
        /// <param name="user">User details for registration.</param>
        /// <returns>API result with registration status.</returns>
        [HttpPost("RegisterUserDetail")]
        public async Task<IActionResult> RegisterUserDetail([FromBody] userMasterModel user)
        {
            ApiResult<userMasterModel> result = new ApiResult<userMasterModel>();
            try
            {
                result = await _logInRepo.RegisterUserDetail(user);

                return result.ResponseCode == 1
                    ? Ok(result)
                    : StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while registering user details.");
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while registering user details.", ex));
            }
        }

        /// <summary>
        /// Login the student by validating email and password.
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Login(LoginRequestModel req)
        {
            ApiResult<LogInResponseModel> result = new ApiResult<LogInResponseModel>();
            try
            {
                // Get student details by email
                result = await _logInRepo.Login(req);
                if (result.ResponseCode == 1)
                {
                    return Ok(result);
                }
                return StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error authenticating user");
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error authenticating user", ex));
            }
        }
    }
}
