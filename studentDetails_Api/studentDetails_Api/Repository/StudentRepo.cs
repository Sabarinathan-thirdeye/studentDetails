using Microsoft.EntityFrameworkCore; // Import Entity Framework Core for ORM functionality.
using studentDetails_Api.Models; // Import project-specific models.
using studentDetails_Api.NonEntity; // Import non-entity data models or helper classes.
using studentDetails_Api.IRepository; // Import repository interface for dependency injection.
using Microsoft.AspNetCore.Mvc; // Import ASP.NET Core MVC attributes for API creation.
using System.Text.RegularExpressions; // Import regular expressions for email validation.
using studentDetails_Api.Services; // Import data namespace, though currently unused.
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
                        firstName = s.firstName,
                        lastName= s.lastName,
                        dateOfBirth = (DateOnly)s.dateOfBirth,  // Ensure this is a valid DateOnly conversion
                        gender = s.gender,
                        email = s.email,
                        mobileNumber = s.mobileNumber,
                        studentstatus = s.studentstatus
                    }).ToList();

                if (studentList.Count > 0)
                {
                    return result.SuccessResponse("Success", studentList);
                }
                else
                {
                    return result.SuccessResponse("No data found", studentList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Retrieves all dacativate student details.
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
                        studentstatus = s.studentstatus
                    }).ToList();

                return studentList.Count > 0 ?
                    result.SuccessResponse("Success", studentList) :
                    result.SuccessResponse("No data found", studentList);
            }
            catch (Exception ex)
            {
                throw ex;
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
                        studentstatus = s.studentstatus
                    }).FirstOrDefault();

                return student != null ?
                    result.SuccessResponse("Success", student) :
                    result.SuccessResponse("No data found", student);
            }
            catch (Exception ex)
            {
                throw ex;
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
                    return result.ValidationErrorResponse("Please provide student details.");

                // Validate fields
                if (string.IsNullOrWhiteSpace(student.firstName))
                    return result.ValidationErrorResponse("Please provide the first name.");
                if (string.IsNullOrWhiteSpace(student.email))
                    return result.ValidationErrorResponse("Please provide an email.");

                 string emailRegexPattern = @"^[\w-]+(\.[\w-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,})$";
                 if (!Regex.IsMatch(student.email, emailRegexPattern))
                    return result.ValidationErrorResponse("Invalid email address format.");

                // Check if the email already exists in the database (excluding the current student if it's an update)
                var existingStudentByEmail = await _context.studentDetails
                    .FirstOrDefaultAsync(s => s.email == student.email && s.studentID != student.studentID);

                if (existingStudentByEmail != null)
                {
                    return result.ValidationErrorResponse("A student with this email already exists.");
                }

                // Check if the student already exists by studentID (for update)
                var existingStudent = await _context.studentDetails
                    .FirstOrDefaultAsync(s => s.studentID == student.studentID);

                if (existingStudent != null)
                {
                    // Update existing record
                    existingStudent.firstName = student.firstName;
                    existingStudent.lastName = student.lastName;
                    existingStudent.email = student.email;
                    existingStudent.mobileNumber = student.mobileNumber;
                    existingStudent.gender = student.gender;
                    existingStudent.dateOfBirth = student.dateOfBirth;
                    existingStudent.studentstatus = student.studentstatus;
                    existingStudent.modifiedBy = 1; // Replace with user ID from claims
                    existingStudent.modifiedOn = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    return result.SuccessResponse("Student updated successfully.", student);
                }
                else
                {
                    // Add new student record
                    var newStudent = new studentDetail
                    {
                        firstName = student.firstName,
                        lastName = student.lastName,
                        email = student.email,
                        gender = student.gender,
                        dateOfBirth = student.dateOfBirth,
                        mobileNumber = student.mobileNumber,
                        studentstatus = student.studentstatus,
                        createdOn = DateTime.UtcNow,
                        createdBy = 1 // Replace with actual user ID from claims
                    };

                    _context.studentDetails.Add(newStudent);
                    await _context.SaveChangesAsync();

                    return result.SuccessResponse("Student registered successfully.", student);
                }
            }
            catch (Exception ex)
            {
                // Log exception or handle it appropriately
                throw ex;
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
                // Find the student by ID
                var student = await _context.studentDetails.FindAsync(studentID);
                if (student == null)
                {
                    return result.ValidationErrorResponse("Student not found.");
                }

                // Check if the student is already inactive
                if (student.studentstatus == 99)
                {
                    result.Message = "Student is already deactivated";
                    return result;
                }

                // Update the student status to 99 (deactivated)
                student.studentstatus = 99;
                student.modifiedBy = 1;  // You can set this to the user who is making the update if needed
                student.modifiedOn = DateTime.UtcNow;  // Set modified date/time

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Return success result
                return result.SuccessResponse("Student status updated successfully.", true);
            }
            catch (Exception ex)
            {
                // Handle exception and log if necessary
                return result.ExceptionResponse("Error while updating student status.", ex);
            }
        }

    }
}
