using studentDetails_Api.IRepository;
using studentDetails_Api.NonEntity;
using Microsoft.AspNetCore.Mvc;
using studentDetails_Api.Models;


namespace studentDetails_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        private readonly ILogInRepo _logInRepo;

        public LogInController(ILogInRepo logInRepo)
        {
            _logInRepo = logInRepo;
        }

        /// <summary>
        /// Login the student by validating email and password.
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestModel loginRequest)
        {
            ApiResult<LogInResponseModel> result = await _logInRepo.GetStudentByEmailAsync(loginRequest);

            if (string.IsNullOrEmpty(loginRequest.email))
            {
                return BadRequest(new { message = "Email and password are required." });

            }
            if (string.IsNullOrEmpty(loginRequest.studentPassword))
            {
                return BadRequest(new { message = "Email and password are required." });

            }
            if (result.ResponseCode == 1)
            {
                return Ok(result); // Return success response
            }

            return Unauthorized(result); // Return Unauthorized if login fails
        }
    }
}
