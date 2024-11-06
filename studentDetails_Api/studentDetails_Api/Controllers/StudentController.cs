using Microsoft.AspNetCore.Mvc;
using studentDetails_Api.IRepository;
using studentDetails_Api.Models;
using studentDetails_Api.NonEntity;

namespace studentDetails_Api.Controllers
{
    /// <summary>
    /// Student Details 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        /// <summary>
        /// Student Repo
        /// </summary>
        private readonly IStudentRepo _studentRepo;


        //private readonly CryptoServices _cryptoServices;
        //private readonly JwtServices _jwtServices;

        //public StudentController(StudentDBContext context, CryptoServices cryptoServices, JwtServices jwtServices)
        //{
        //    _context = context;
        //    _cryptoServices = cryptoServices;
        //    _jwtServices = jwtServices;
        //}

        /// <summary>
        /// Student Repo
        /// </summary>
        /// <param name="studentRepo"></param>
        public StudentController(IStudentRepo studentRepo)
        {
            _studentRepo = studentRepo;
        }
        //public StudentController(StudentDBContext context, CryptoServices cryptoServices)
        //{
        //    _context = context;
        //    _cryptoServices = cryptoServices;
        //}
        /// <summary>
        /// Retrive the All Student Details 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            ApiResult<studentDetailModel> result = new ApiResult<studentDetailModel>();
            try
            {
                result = _studentRepo.GetStudentDetails();
                if (result.ResponseCode == 1)
                {
                    return Ok(result);
                }
                return StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("We don't have a student details ", ex)); ;
            }

        }

        /// <summary>
        /// Retrieves Company details by Id
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

                if (result.ResponseCode == 1)
                {
                    return Ok(result);
                }

                return StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while retrieving a student.", ex)); ;
            }
        }


        /// <summary>
        /// Retrieves Company details by Id
        /// </summary>
        /// <param name="studentID"></param>
        /// <returns></returns>
        [HttpPost("AddorStudentStudentDetails")]
        public async Task<IActionResult> AddorUpdateStudentDetails(studentDetailModel details)
        {
            ApiResult<studentDetailModel> result = new ApiResult<studentDetailModel>();
            try
            {
                result = await _studentRepo.AddorupdateStudentDetails(details); ; // Await the asynchronous operation

                if (result.ResponseCode == 1)
                {
                    return Ok(result);
                }
                return StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while add or update a company.", ex)); ;
            }
        }



        /// <summary>
        /// Deactivate student account by ID
        /// </summary>
        /// <param name="studentID"></param>
        /// <returns></returns>
        [HttpPost("Deactivate/{id}")]
        public async Task<IActionResult> DeleteStudentDetails(int id)
        {
            ApiResult<studentDetailModel> result;

            try
            {
                // Await the asynchronous delete operation
                result = await _studentRepo.DeleteStudentDetails(id);

                // Handle the response based on ResponseCode
                if (result.ResponseCode == 1)
                {
                    return Ok(result); // Success
                }
                else if (result.ResponseCode == 0)
                {
                    return NotFound(result); // Student not found
                }
                else if (result.ResponseCode == -1)
                {
                    return StatusCode(StatusCodes.Status412PreconditionFailed, result); // Precondition failed
                }

                return StatusCode(StatusCodes.Status500InternalServerError, result); // Handle other errors
            }
            catch (Exception ex)
            {
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResult<studentDetailModel>
                    {
                        ResponseCode = -1,
                        Message = "An error occurred while processing your request.",
                        ErrorDesc = ex.Message
                    });
            }
        }


    }


}


