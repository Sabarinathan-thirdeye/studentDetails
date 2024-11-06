using Microsoft.AspNetCore.Mvc;
using UsersDetailsApi.NonEntity;
using UsersDetailsApi.Models;
using UsersDetailsApi.IRepository;
using System;
using System.Threading.Tasks;

namespace UsersDetailsApi.Controllers
{
    /// <summary>
    /// Student Details 
    /// </summary>
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userRepo">The user repository.</param>
        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        /// <summary>
        /// Retrieves all user details.
        /// </summary>
        /// <returns>An action result containing the user details.</returns>
        [HttpGet("users")]
        /// <summary>
        /// Retrive the All Student Details 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserDetails()
        {
            ApiResult<UserModel> result = new ApiResult<UserModel>();
            try
            {
                result = _userRepo.GetUserDetails();
                if (result.ResponseCode == 1)
                {
                    return Ok(result);
                }
                return StatusCode(StatusCodes.Status412PreconditionFailed, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while retriving an user details ", ex)); ;
            }

        }

        ///// <summary>
        ///// Retrieves Company details by Id
        ///// </summary>
        ///// <param name="studentID"></param>
        ///// <returns></returns>
        //[HttpGet("GetStudent/{studentID}")]
        //public IActionResult GetStudentDetailsbyID(long studentID)
        //{
        //    ApiResult<UserTypeMasterModel> result = new ApiResult<UserTypeMasterModel>();
        //    try
        //    {
        //        result = _studentRepo.GetStudentDetailsbyID(studentID);

        //        if (result.ResponseCode == 1)
        //        {
        //            return Ok(result);
        //        }

        //        return StatusCode(StatusCodes.Status412PreconditionFailed, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while retrieving a student.", ex)); ;
        //    }
        //}


        //    /// <summary>
        //    /// Retrieves Company details by Id
        //    /// </summary>
        //    /// <param name="studentID"></param>
        //    /// <returns></returns>
        //    [HttpPost("AddorStudentCompany")]
        //    public async Task<IActionResult> AddorUpdateStudentDetails(UserTypeMasterModel details)
        //    {
        //        ApiResult<UserTypeMasterModel> result = new ApiResult<UserTypeMasterModel>();
        //        try
        //        {
        //            result = await _studentRepo.AddorupdateStudentDetails(details); ; // Await the asynchronous operation

        //            if (result.ResponseCode == 1)
        //            {
        //                return Ok(result);
        //            }
        //            return StatusCode(StatusCodes.Status412PreconditionFailed, result);
        //        }
        //        catch (Exception ex)
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while add or update a company.", ex)); ;
        //        }
        //    }
        //}


        /// <summary>
        /// Deactivate company account by Id
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        //[HttpPost("Deactivate/{companyID}")]
        //public async Task<IActionResult> DeactivateCompanyAccountById(long companyID)
        //{
        //    ApiResult<bool> result = new ApiResult<bool>();
        //    try
        //    {
        //        result = await _companyRepo.DeactivateCompanyAccountById(companyID);

        //        if (result.ResponseCode == 1)
        //        {
        //            return Ok(result);
        //        }
        //        return StatusCode(StatusCodes.Status412PreconditionFailed, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogErrorDetails(ex, ex.Message, _contextAccessor, companyID, result.ExceptionResponse("Error while deleting a company.", ex));
        //        return StatusCode(StatusCodes.Status500InternalServerError, result.ExceptionResponse("Error while deleting a company.", ex)); ;
        //    }

        //}

    }
}

