using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;                       // Import MVC functionalities for controller
using studentDetails_Api.Services;                           // Import custom services for the project
using studentDetails_Api.IRepository;                 // Import repository interface for student details
using studentDetails_Api.Models;                      // Import models related to student details
using studentDetails_Api.NonEntity;                   // Import non-entity classes for handling non-database responses

namespace studentDetails_Api.Controllers              // Define the namespace for the controller
{
    // Define the route as "api/Student" and mark as API controller for automatic model validation
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase   // Inherit ControllerBase to make this an API controller
    {
        // Private fields for dependency injection
        private readonly IStudentRepo _studentRepo;   // Interface for student repository
        private readonly IHttpContextAccessor _contextAccessor; // Interface for accessing HTTP context data
        private readonly IConfiguration _config;      // Configuration interface for app settings
        //private readonly CryptoServices _cryptoService; // Service for encrypting and decrypting data

        // Constructor to inject dependencies via dependency injection
        public StudentController(IStudentRepo studentRepo, IHttpContextAccessor contextAccessor, IConfiguration config)
        {
            _studentRepo = studentRepo;               // Initialize repository interface
            _contextAccessor = contextAccessor;       // Initialize HTTP context accessor
            _config = config;                         // Initialize configuration interface
            //_cryptoService = new CryptoServices(_config, _contextAccessor); // Initialize CryptoServices with config
        }

        /// <summary>
        /// GET method to retrieve all active student details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetStudentDetails()
        {
            ApiResult<studentDetailModel> result = new ApiResult<studentDetailModel>();
            try
            {
                result = _studentRepo.GetStudentDetails();

                return result.ResponseCode == 1 ? Ok(result) : StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                //_logger.LogErrorDetails(ex, ex.Message, _contextAccessor, "", result.ExceptionResponse("Error while retriving an company details ", ex));
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while retriving an student details ", ex)); ;
            }
        }

        /// <summary>
        /// GET method to retrieve all inactive student details
        /// </summary>
        /// <returns></returns>
        [HttpGet("InActive")]
        public IActionResult GetAllStudentsInActive()
        {
            ApiResult<studentDetailModel> result = new ApiResult<studentDetailModel>();
            try
            {
                result = _studentRepo.GetStudentDetailsInActive();
                return result.ResponseCode == 1 ? Ok(result) : StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                //_logger.LogErrorDetails(ex, ex.Message, _contextAccessor, result.ExceptionResponse("Error while retrieving a inactive student details.", ex));
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while retrieving a student.", ex)); ;
            }
        }
        /// <summary>
        /// GET method to retrieve a specific student's details by student ID
        /// </summary>
        /// <param name="studentID"></param>
        /// <returns></returns>
        [HttpGet("GetStudent/{studentID}")]
        public IActionResult GetStudentDetailsbyID(long studentID)
        {
            ApiResult<studentDetailModel> result = new ApiResult<studentDetailModel>();
            try
            {
                result = _studentRepo.GetStudentDetailsbyID(studentID);
                return result.ResponseCode == 1 ? Ok(result) : StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                //_logger.LogErrorDetails(ex, ex.Message, _contextAccessor, studentID, result.ExceptionResponse("Error while retrieving a student.", ex));
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while retrieving a student.", ex)); ;
            }
        }

        /// <summary>
        /// POST method to add or update a student's details
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost("AddOrUpdateStudentDetails")]
        public async Task<IActionResult> AddOrUpdateStudentDetails(studentDetailModel student)
        {
            ApiResult<studentDetailModel> result = new ApiResult<studentDetailModel>();
            try
            {
                result = await _studentRepo.AddOrUpdateStudentDetails(student);
                return result.ResponseCode == 1 ? Ok(result) : StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                //_logger.LogErrorDetails(ex, ex.Message, _contextAccessor, "", result.ExceptionResponse("Error while add or update a student.", ex));
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while add or update a student.", ex)); ;
            }
        }

        /// <summary>
        /// POST method to deactivate a student by ID, setting studentStatus to 99
        /// </summary>
        /// <param name="studentID"></param>
        /// <returns></returns>
        [HttpPost("Deactivate/{id}")]
        public async Task<IActionResult> UpdateStudentStatusAsync(long studentID)
        {
            ApiResult<bool> result = new ApiResult<bool>();
            try
            {
                result = await _studentRepo.UpdateStudentStatusAsync(studentID);
                return result.ResponseCode == 1 ? Ok(result) : StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                //_logger.LogErrorDetails(ex, ex.Message, _contextAccessor, id, result.ExceptionResponse("Error while deleting a student.", ex));
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while deleting a student.", ex)); ;
            }
        }
    }
}
