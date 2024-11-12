using Microsoft.EntityFrameworkCore; // Import Entity Framework Core for ORM functionality.
using studentDetails_Api.Models; // Import project-specific models.
using studentDetails_Api.NonEntity; // Import non-entity data models or helper classes.
using studentDetails_Api.IRepository; // Import repository interface for dependency injection.
using Microsoft.AspNetCore.Mvc; // Import ASP.NET Core MVC attributes for API creation.
using System.Text.RegularExpressions; // Import regular expressions, though currently unused.
using System.Data;
using studentDetails_Api.Services; // Import data namespace, though currently unused.

namespace studentDetails_Api.Repository
{
    /// <summary>
    /// Repository for performing operations related to student details.
    /// </summary>
    public class StudentRepo : IStudentRepo
    {
        /// <summary>
        /// DBContext
        /// </summary>
        private readonly StudentDBContext _context;
        /// <summary>
        /// Current Http ContextAccessor
        /// </summary>
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly CryptoServices _cryptoServices;

        /// <summary>
        /// Initializes a new instance of <see cref="StudentRepo"/>.
        /// </summary>
        /// <param name="context">The database context for database interactions.</param>
        /// <param name="contextAccessor">Accessor for the current HTTP context.</param>
        public StudentRepo(StudentDBContext context, IHttpContextAccessor contextAccessor,CryptoServices cryptoServices)
        {
            _context = context; 
            _contextAccessor = contextAccessor;
            _cryptoServices = cryptoServices;
        }

        /// <summary>
        /// Retrieves all active student details from the database.
        /// </summary>
        public ApiResult<studentDetailModel> GetStudentDetails()
        {
            ApiResult<studentDetailModel> result = new ApiResult<studentDetailModel>(); 
            try
            {
                //var claimData = _contextAccessor.HttpContext!.Items["ClaimData"] as ClaimData;
                var StudentList = _context.studentDetails
                    .Where(u => u.studentstatus != 99) 
                    .Select(s => new studentDetailModel 
                    {
                        studentID = s.studentID,
                        studentName = s.studentName,
                        userName = s.userName,
                        dateOfBirth = (DateOnly)s.dateOfBirth,
                        gender = s.gender,
                        email = s.email,
                        mobileNumber = s.mobileNumber,
                        createdOn = s.createdOn,
                        createdBy = s.createdBy,
                        modifiedBy = s.modifiedBy,
                        modifiedOn = s.modifiedOn,
                        studentPassword = s.studentPassword,
                        studentstatus = s.studentstatus
                    }).ToList();

                return StudentList.Count > 0 ?
                    result.SuccessResponse("Success", StudentList) :
                    result.SuccessResponse("No data found", StudentList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves all inactive student details.
        /// </summary>
        public ApiResult<studentDetailModel> GetStudentDetailsInActive()
        {
            ApiResult<studentDetailModel> result = new ApiResult<studentDetailModel>();
            try
            {
                //var claimData = _contextAccessor.HttpContext?.Items["ClaimData"] as ClaimData;
                var StudentList = _context.studentDetails
                    .Where(u => u.studentstatus == 99)
                    .Select(s => new studentDetailModel
                    {
                        studentID = s.studentID,
                        studentName = s.studentName,
                        userName = s.userName,
                        dateOfBirth = (DateOnly)s.dateOfBirth,
                        gender = s.gender,
                        email = s.email,
                        mobileNumber = s.mobileNumber,
                        createdOn = s.createdOn,
                        createdBy = s.createdBy,
                        modifiedBy = s.modifiedBy,
                        modifiedOn = s.modifiedOn,
                        studentPassword = s.studentPassword,
                        studentstatus = s.studentstatus
                    }).ToList();

                return StudentList.Count > 0 ?
                    result.SuccessResponse("Success", StudentList) :
                    result.SuccessResponse("No data found", StudentList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves details of a student by their ID if the student is active.
        /// </summary>
        /// <param name="studentID">ID of the student to retrieve.</param>
        public ApiResult<studentDetailModel> GetStudentDetailsbyID(long studentID)
        {
            ApiResult<studentDetailModel> result = new ApiResult<studentDetailModel>();
            try
            {
                //var claimData = _contextAccessor.HttpContext!.Items["ClaimData"] as ClaimData;
                var student = _context.studentDetails
                    .Where(u => u.studentID == studentID && u.studentstatus != 99)
                    .Select(s => new studentDetailModel
                    {
                        studentID = s.studentID,
                        studentName = s.studentName,
                        userName = s.userName,
                        dateOfBirth = s.dateOfBirth,
                        gender = s.gender,
                        email = s.email,
                        mobileNumber = s.mobileNumber,
                        createdOn = s.createdOn,
                        createdBy = s.createdBy,
                        modifiedBy = s.modifiedBy,
                        modifiedOn = s.modifiedOn,
                        studentPassword = s.studentPassword,
                        studentstatus = s.studentstatus
                    }).FirstOrDefault(); 

                return student != null ?
                    result.SuccessResponse("Success", student) :
                    result.SuccessResponse("No data found", student);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the student status to inactive (99) by student ID.
        /// </summary>
        /// <param name="studentID">ID of the student to deactivate.</param>
        public async Task<ApiResult<bool>> UpdateStudentStatusAsync(long studentID)
        {
            ApiResult<bool> result = new ApiResult<bool>();
            try
            {
                var claimData = _contextAccessor.HttpContext!.Items["ClaimData"] as ClaimData;
                // Find the student by ID.
                var student = await _context.studentDetails.FindAsync(studentID);
                if (student == null)
                {
                    return result.ValidationErrorResponse("employee not found.");

                }
                                // Checks if the student is already inactive.
                if (student.studentstatus == 99)
                {
                    result.Message = "Student is already deactivated";
                    return result;
                }
                else
                {
                    student.modifiedBy = claimData!.UserID;
                    student.modifiedOn = DateTime.UtcNow;
                    student.studentstatus = 99;
                    _context.SaveChanges();

                    return result.SuccessResponse("Deleted Successfully", true);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
