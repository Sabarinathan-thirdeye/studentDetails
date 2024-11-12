using Microsoft.AspNetCore.Mvc;
using studentDetails_Api.IRepository;
using studentDetails_Api.Models;
using studentDetails_Api.NonEntity;
using studentDetails_Api.Services;
using System.Security.Claims;


namespace studentDetails_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        private readonly ILogInRepo _logInRepo;
        private readonly ILogger<LogInController> _logger;
        private JwtServices _jwtServices;

        public LogInController(ILogInRepo logInRepo, JwtServices jwtServices, ILogger<LogInController> logger)
        {
            _logInRepo = logInRepo;
            _jwtServices = jwtServices;
            _logger = logger;
        }

        /// <summary>
        /// Login the student by validating email and password.
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost("Authenticate")]
        public async Task<IActionResult> GetStudentByEmailAsync(LoginRequestModel req)
        {
            ApiResult<LogInResponseModel> result = new ApiResult<LogInResponseModel>();
            try
            {
                // Get student details by email
                result = await _logInRepo.GetStudentByEmailAsync(req);
                if (result.ResponseCode == 1)
                {
                    // Generate JWT token if credentials are valid
                    var student = result.Data; // Assuming result.Data contains the student details
                    var token = _jwtServices.GenerateToken(student); // Generate the token

                    return Ok(new
                    {
                        Token = token,
                        Student = student
                    });
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