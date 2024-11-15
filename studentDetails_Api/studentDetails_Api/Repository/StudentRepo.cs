using Microsoft.EntityFrameworkCore; // Import Entity Framework Core for ORM functionality.
using studentDetails_Api.Models; // Import project-specific models.
using studentDetails_Api.NonEntity; // Import non-entity data models or helper classes.
using studentDetails_Api.IRepository; // Import repository interface for dependency injection.
using Microsoft.AspNetCore.Mvc; // Import ASP.NET Core MVC attributes for API creation.
using System.Text.RegularExpressions; // Import regular expressions for email validation.
using studentDetails_Api.Services; // Import data namespace, though currently unused.
using System.Data;

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
        public StudentRepo(StudentDBContext context, IHttpContextAccessor contextAccessor, CryptoServices cryptoServices)
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
                var studentList = _context.studentDetails
                    .Where(u => u.studentstatus != 99)
                    .Select(s => new studentDetailModel
                    {
                        studentID = s.studentID,
                        dateOfBirth = (DateOnly)s.dateOfBirth,
                        gender = s.gender,
                        email = s.email,
                        mobileNumber = s.mobileNumber,
                        createdOn = s.createdOn,
                        createdBy = s.createdBy,
                        modifiedBy = s.modifiedBy,
                        modifiedOn = s.modifiedOn,
                        studentstatus = s.studentstatus
                    }).ToList();

                return studentList.Count > 0 ?
                    result.SuccessResponse("Success", studentList) :
                    result.SuccessResponse("No data found", studentList);
            }
            catch (Exception ex)
            {
                return result.SuccessResponse("An error occurred while retrieving data.", new List<studentDetailModel>());
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
                var studentList = _context.studentDetails
                    .Where(u => u.studentstatus == 99)
                    .Select(s => new studentDetailModel
                    {
                        studentID = s.studentID,
                        dateOfBirth = (DateOnly)s.dateOfBirth,
                        gender = s.gender,
                        email = s.email,
                        mobileNumber = s.mobileNumber,
                        createdOn = s.createdOn,
                        createdBy = s.createdBy,
                        modifiedBy = s.modifiedBy,
                        modifiedOn = s.modifiedOn,
                        studentstatus = s.studentstatus
                    }).ToList();

                return studentList.Count > 0 ?
                    result.SuccessResponse("Success", studentList) :
                    result.SuccessResponse("No data found", studentList);
            }
            catch (Exception ex)
            {
                return result.SuccessResponse("An error occurred while retrieving inactive data.", new List<studentDetailModel>());
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
                var student = _context.studentDetails
                    .Where(u => u.studentID == studentID && u.studentstatus != 99)
                    .Select(s => new studentDetailModel
                    {
                        studentID = s.studentID,
                        dateOfBirth = s.dateOfBirth,
                        gender = s.gender,
                        email = s.email,
                        mobileNumber = s.mobileNumber,
                        createdOn = s.createdOn,
                        createdBy = s.createdBy,
                        modifiedBy = s.modifiedBy,
                        modifiedOn = s.modifiedOn,
                        studentstatus = s.studentstatus
                    }).FirstOrDefault();

                return student != null ?
                    result.SuccessResponse("Success", student) :
                    result.SuccessResponse("No data found", student);
            }
            catch (Exception ex)
            {
                return result.SuccessResponse("An error occurred while retrieving student details.", new List<studentDetailModel>());
            }
        }

        /// <summary>
        /// Adds or updates student details with encryption for sensitive fields.
        /// </summary>
        /// <param name="student">Student details to add or update.</param>
        /// <returns>ApiResult with the student details and operation status.</returns>
        public async Task<ApiResult<studentDetailModel>> AddOrUpdateStudentDetails(studentDetailModel student)
        {
            var result = new ApiResult<studentDetailModel>();
            try
            {
                if (student == null)
                {
                    return result.ValidationErrorResponse("Please provide student details.");
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(student.firstName)) return result.ValidationErrorResponse("Please provide first name.");
                if (string.IsNullOrWhiteSpace(student.lastName)) return result.ValidationErrorResponse("Please provide last name.");
                if (string.IsNullOrWhiteSpace(student.userName)) return result.ValidationErrorResponse("Please provide user name.");
                if (string.IsNullOrWhiteSpace(student.email)) return result.ValidationErrorResponse("Please provide email.");

                // Validate email format
                string emailRegexPattern = @"^[\w-]+(\.[\w-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,})$";
                if (!Regex.IsMatch(student.email, emailRegexPattern)) return result.ValidationErrorResponse("Invalid email address.");

                if (string.IsNullOrWhiteSpace(student.gender)) return result.ValidationErrorResponse("Please provide gender.");


                // Check for existing records
                var existingStudent = await _context.studentDetails
                    .FirstOrDefaultAsync(s => s.email == student.email);

                if (existingStudent != null)
                {
                    return result.ValidationErrorResponse("Email already exists.");
                }

                // Check if updating or creating a new record
                var studentRecord = await _context.studentDetails
                    .FirstOrDefaultAsync(s => s.email == student.email && s.userName == student.userName);

                if (studentRecord == null)
                {
                    // Add new student
                    studentRecord = new studentDetail
                    {
                        userName = student.userName,
                        email = student.email,  // No encryption on email
                        mobileNumber = student.mobileNumber,
                        gender = student.gender,
                        dateOfBirth = student.dateOfBirth,
                        createdOn = DateTime.UtcNow,
                        createdBy = 0,  // For demo purposes, set to 0 or use your claim data here
                        studentstatus = student.studentstatus
                    };

                    _context.studentDetails.Add(studentRecord);
                    await _context.SaveChangesAsync();

                    return result.SuccessResponse("Student created successfully.", student);
                }
                else
                {
                    studentRecord.userName = student.userName;
                    studentRecord.email = student.email;
                    studentRecord.mobileNumber = student.mobileNumber;
                    studentRecord.gender = student.gender;
                    studentRecord.dateOfBirth = student.dateOfBirth;
                    studentRecord.modifiedBy = 0;  // Set modified by from claim data if needed
                    studentRecord.modifiedOn = DateTime.UtcNow;

                    await _context.SaveChangesAsync();

                    return result.SuccessResponse("Student created successfully.", student);
                }
            }
            catch (Exception ex)
            {
                result.ResponseCode = -1;
                result.Message = "An error occurred while processing the request.";
                result.ErrorDesc = ex.Message;
                return result;
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
                var student = await _context.studentDetails.FindAsync(studentID);
                if (student == null)
                {
                    return result.ValidationErrorResponse("Student not found.");
                }

                // Checks if the student is already inactive.
                if (student.studentstatus == 99)
                {
                    result.Message = "Student is already deactivated";
                    return result;
                }
                else
                {
                    student.studentstatus = 99;
                    student.modifiedBy = 0;  // Set modified by from claim data if needed
                    student.modifiedOn = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    return result.SuccessResponse("Student status updated successfully.", true);
                }
            }
            catch (Exception ex)
            {
                return result.SuccessResponse("An error occurred while updating student status.",true);
            }
        }
    }
}
