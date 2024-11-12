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
        private readonly CryptoServices _cryptoService; // Service for encrypting and decrypting data

        // Constructor to inject dependencies via dependency injection
        public StudentController(IStudentRepo studentRepo, IHttpContextAccessor contextAccessor, IConfiguration config)
        {
            _studentRepo = studentRepo;               // Initialize repository interface
            _contextAccessor = contextAccessor;       // Initialize HTTP context accessor
            _config = config;                         // Initialize configuration interface
            _cryptoService = new CryptoServices(_config, _contextAccessor); // Initialize CryptoServices with config
        }

        // GET method to retrieve all active student details
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            try
            {
                // Call the repository method to get all active students
                var result = _studentRepo.GetStudentDetails();

                // Check response code; if success, return OK, else return 412 Precondition Failed
                return result.ResponseCode == 1 ? Ok(result) : StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error with error details if an exception occurs
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult<studentDetailModel>
                {
                    ResponseCode = -1,                 // Set custom error code
                    Message = "Error while retrieving all student details.", // Custom error message
                    ErrorDesc = ex.Message            // Provide exception message for debugging
                });
            }
        }

        // GET method to retrieve all inactive student details
        [HttpGet("InActive")]
        public IActionResult GetAllStudentsInActive()
        {
            try
            {
                // Call the repository method to get all inactive students
                var result = _studentRepo.GetStudentDetailsInActive();

                // Check response code; if success, return OK, else return 412 Precondition Failed
                return result.ResponseCode == 1 ? Ok(result) : StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error with error details if an exception occurs
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult<studentDetailModel>
                {
                    ResponseCode = -1,                 // Set custom error code
                    Message = "Error while retrieving inactive student details.", // Custom error message
                    ErrorDesc = ex.Message            // Provide exception message for debugging
                });
            }
        }

        // GET method to retrieve a specific student's details by student ID
        [HttpGet("GetStudent/{studentID}")]
        public IActionResult GetStudentDetailsbyID(long studentID)
        {
            try
            {
                // Call the repository method to get student details by ID
                var result = _studentRepo.GetStudentDetailsbyID(studentID);

                // Check response code; if success, return OK, else return 412 Precondition Failed
                return result.ResponseCode == 1 ? Ok(result) : StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error with error details if an exception occurs
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult<studentDetailModel>
                {
                    ResponseCode = -1,                 // Set custom error code
                    Message = "Error while retrieving a student.", // Custom error message
                    ErrorDesc = ex.Message            // Provide exception message for debugging
                });
            }
        }

        // POST method to add or update a student's details
        [HttpPost("AddorUpdateStudentDetails")]
        public async Task<IActionResult> AddorUpdateStudentDetails([FromBody] studentDetailModel details)
        {
            // Check if the incoming model data is valid
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");    // Return 400 Bad Request if data is invalid
            }

            try
            {
                // Encrypt the student password before storing in the database
                details.studentPassword = _cryptoService.EncryptStringToBytes_Aes(details.studentPassword);

                // Call repository method to add or update student details asynchronously
                var result = await _studentRepo.AddorupdateStudentDetails(details);

                // Check response code; if success, return OK, else return 412 Precondition Failed
                return result.ResponseCode == 1 ? Ok(result) : StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error with error details if an exception occurs
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult<studentDetailModel>
                {
                    ResponseCode = -1,                 // Set custom error code
                    Message = "Error while adding or updating student details.", // Custom error message
                    ErrorDesc = ex.Message            // Provide exception message for debugging
                });
            }
        }

        // POST method to deactivate a student by ID, setting studentStatus to 99
        [HttpPost("Deactivate/{id}")]
        public async Task<IActionResult> DeleteStudentDetails(int id)
        {
            try
            {
                // Call repository method to update student status to inactive (99)
                var result = await _studentRepo.UpdateStudentStatusAsync(id);

                // Check response code and return appropriate status
                return result.ResponseCode switch
                {
                    1 => Ok(result),                    // Return 200 OK if deactivation was successful
                    0 => NotFound(result),              // Return 404 Not Found if student not found
                    -1 => StatusCode(StatusCodes.Status412PreconditionFailed, result), // Return 412 if precondition failed
                    _ => StatusCode(StatusCodes.Status500InternalServerError, result) // Return 500 for any other error
                };
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error with error details if an exception occurs
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult<studentDetailModel>
                {
                    ResponseCode = -1,                 // Set custom error code
                    Message = "An error occurred while processing your request.", // Custom error message
                    ErrorDesc = ex.Message            // Provide exception message for debugging
                });
            }
        }
    }
}
