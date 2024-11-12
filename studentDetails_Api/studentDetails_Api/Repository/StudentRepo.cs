using Microsoft.EntityFrameworkCore; // Import Entity Framework Core for ORM functionality.
using studentDetails_Api.Models; // Import project-specific models.
using studentDetails_Api.NonEntity; // Import non-entity data models or helper classes.
using studentDetails_Api.IRepository; // Import repository interface for dependency injection.
using Microsoft.AspNetCore.Mvc; // Import ASP.NET Core MVC attributes for API creation.
using System.Text.RegularExpressions; // Import regular expressions, though currently unused.
using System.Data; // Import data namespace, though currently unused.

namespace studentDetails_Api.Repository
{
    /// <summary>
    /// Repository for performing operations related to student details.
    /// </summary>
    public class StudentRepo : IStudentRepo
    {
        // Dependency-injected context for database operations.
        private readonly StudentDBContext _context;

        // Provides access to the current HTTP context, often for user or session info.
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Initializes a new instance of <see cref="StudentRepo"/>.
        /// </summary>
        /// <param name="context">The database context for database interactions.</param>
        /// <param name="contextAccessor">Accessor for the current HTTP context.</param>
        public StudentRepo(StudentDBContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context; // Assigns the injected database context to the private variable.
            _contextAccessor = contextAccessor; // Assigns the injected HTTP context accessor to the private variable.
        }

        /// <summary>
        /// Retrieves all active student details from the database.
        /// </summary>
        public ApiResult<studentDetailModel> GetStudentDetails()
        {
            ApiResult<studentDetailModel> result = new ApiResult<studentDetailModel>(); // Result container for API responses.
            try
            {
                // Retrieve a list of students who are active (status != 99).
                var StudentList = _context.studentDetails
                    .Where(u => u.studentstatus != 99) // Filters active students.
                    .Select(s => new studentDetailModel // Projects data into studentDetailModel.
                    {
                        studentID = s.studentID,
                        firstName = s.firstName,
                        lastName = s.lastName,
                        dateOfBirth = (DateOnly)s.dateOfBirth, // Converts DateTime to DateOnly.
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

                // Checks if data was found and sets the response accordingly.
                return StudentList.Count > 0 ?
                    result.SuccessResponse("Success", StudentList) :
                    result.SuccessResponse("No data found", StudentList);
            }
            catch (Exception ex)
            {
                // Returns an exception response in case of error.
                return result.ExceptionResponse("Error retrieving student details.", ex);
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
                // Access claim data if needed (contextAccessor should handle claims data).
                var claimData = _contextAccessor.HttpContext?.Items["ClaimData"] as ClaimData;

                // Retrieve students with a status of 99 (inactive).
                var StudentList = _context.studentDetails
                    .Where(u => u.studentstatus == 99)
                    .Select(s => new studentDetailModel
                    {
                        studentID = s.studentID,
                        firstName = s.firstName,
                        lastName = s.lastName,
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

                // Sets the response based on retrieved data.
                return StudentList.Count > 0 ?
                    result.SuccessResponse("Success", StudentList) :
                    result.SuccessResponse("No data found", StudentList);
            }
            catch (Exception ex)
            {
                return result.ExceptionResponse("Error retrieving student details.", ex);
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
                        firstName = s.firstName,
                        lastName = s.lastName,
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
                    }).FirstOrDefault(); // Return a single matching student or null if none found.

                // Checks if student data was found and sets the response.
                return student != null ?
                    result.SuccessResponse("Success", student) :
                    result.SuccessResponse("No data found", student);
            }
            catch (Exception ex)
            {
                return result.ExceptionResponse("Error retrieving student details.", ex);
            }
        }

        /// <summary>
        /// Adds or updates student details.
        /// </summary>
        /// <param name="role">Student details to add or update.</param>
        public async Task<ApiResult<studentDetailModel>> AddorupdateStudentDetails(studentDetailModel role)
        {
            ApiResult<studentDetailModel> result = new ApiResult<studentDetailModel>();
            try
            {
                var existingStudent = _context.studentDetails.FirstOrDefault(a => a.studentID == role.studentID);

                if (existingStudent != null)
                {
                    // Update student details if they already exist.
                    existingStudent.firstName = role.firstName;
                    existingStudent.lastName = role.lastName;
                    existingStudent.email = role.email;
                    existingStudent.mobileNumber = role.mobileNumber;
                    existingStudent.gender = role.gender;
                    existingStudent.dateOfBirth = role.dateOfBirth;
                    existingStudent.modifiedBy = role.modifiedBy;
                    existingStudent.modifiedOn = DateTime.Now; // Updates modification timestamp.
                    existingStudent.studentPassword = role.studentPassword; // Password should be encrypted.

                    await _context.SaveChangesAsync();
                    return result.SuccessResponse("Updated successfully.", role);
                }
                else
                {
                    // Create a new student entry if it does not exist.
                    var newStudent = new studentDetail
                    {
                        firstName = role.firstName,
                        lastName = role.lastName,
                        dateOfBirth = role.dateOfBirth,
                        gender = role.gender,
                        email = role.email,
                        mobileNumber = role.mobileNumber,
                        createdOn = DateTime.Now,
                        createdBy = role.createdBy,
                        modifiedOn = DateTime.Now,
                        modifiedBy = role.modifiedBy,
                        studentPassword = role.studentPassword, // Password should be encrypted.
                        studentstatus = role.studentstatus
                    };

                    _context.studentDetails.Add(newStudent);
                    await _context.SaveChangesAsync();
                    return result.SuccessResponse("Created successfully.", role);
                }
            }
            catch (Exception ex)
            {
                result.ResponseCode = -1;
                result.Message = "Error occurred during the operation.";
                result.ErrorDesc = ex.Message;
                return result;
            }
        }

        /// <summary>
        /// Sets the student status to inactive (99) by student ID.
        /// </summary>
        /// <param name="studentID">ID of the student to deactivate.</param>
        public async Task<ApiResult<studentDetailModel>> UpdateStudentStatusAsync(long studentID)
        {
            var result = new ApiResult<studentDetailModel>();
            try
            {
                // Find the student by ID.
                var student = await _context.studentDetails.FindAsync(studentID);

                if (student == null)
                {
                    result.ResponseCode = 0;
                    result.Message = "Student not found";
                    return result;
                }

                // Checks if the student is already inactive.
                if (student.studentstatus == 99)
                {
                    result.Message = "Student is already deactivated";
                    return result;
                }

                // Set student status to inactive (99) and save changes.
                student.studentstatus = 99;
                _context.Update(student);
                await _context.SaveChangesAsync();

                result.ResponseCode = 1;
                result.Message = "Student status updated to inactive successfully";
                return result;
            }
            catch (Exception ex)
            {
                result.ResponseCode = -1;
                result.Message = "An error occurred while updating the student status";
                result.ErrorDesc = ex.Message;
                return result;
            }
        }
    }
}
