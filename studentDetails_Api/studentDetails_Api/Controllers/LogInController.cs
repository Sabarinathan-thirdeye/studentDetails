using Microsoft.AspNetCore.Mvc;
using studentDetails_Api.IRepository;
using studentDetails_Api.Models;
using studentDetails_Api.NonEntity;
using studentDetails_Api.Repository;
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
        /// POST method to add or update a student's details
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        [HttpPost("AddorUpdateStudentDetails")]
        public async Task<IActionResult> AddorUpdateStudentDetails([FromBody] studentDetailModel student)
        {
            ApiResult<studentDetailModel> result = new ApiResult<studentDetailModel>();
            try
            {
                result = await _logInRepo.AddOrUpdateStudentDetails(student);
                return result.ResponseCode == 1 ? Ok(result) : StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                //_logger.LogErrorDetails(ex, ex.Message, _contextAccessor, "", result.ExceptionResponse("Error while add or update a student.", ex));
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while add or update a student.", ex)); ;
            }
        }

        /// <summary>
        /// Login the student by validating email and password.
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Auth(LoginRequestModel req)
        {
            ApiResult<LogInResponseModel> result = new ApiResult<LogInResponseModel>();
            try
            {
                // Get student details by email
                result = await _logInRepo.GetStudentByEmailAsync(req);
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