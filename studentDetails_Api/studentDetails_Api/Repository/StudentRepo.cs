using Microsoft.EntityFrameworkCore;
using studentDetails_Api.Models;
using studentDetails_Api.NonEntity;
using studentDetails_Api.IRepository;

namespace studentDetails_Api.Repository
{
    /// <summary>
    /// Repository for STUDENT DETAILS operations.
    /// </summary>
    public class StudentRepo : IStudentRepo
    /// <summary>
    /// Represents the database context for interacting with the database.
    /// </summary>
    {
        private readonly StudentDBContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentRepo"/> class.
        /// </summary>
        /// <param name="context">The database context used for database operations.</param>
        public StudentRepo(StudentDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all user details 
        /// </summary>
        public ApiResult<studentDetailModel> GetStudentDetails()
        {
            ApiResult<studentDetailModel> result = new ApiResult<studentDetailModel>();
            try
            {
                var StudentList = _context.studentDetails
                    .Where(u => u.studentstatus != 99)
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
                        createBy = s.createBy,
                        modifiedBy = s.modifiedBy,
                        modifiedOn = s.modifiedOn,
                        studentPassword = s.studentPassword,
                        studentstatus = s.studentstatus
                    }).ToList();

                if (StudentList.Count > 0)
                {
                    return result.SuccessResponse("Success", StudentList);
                }
                return result.SuccessResponse("No data found", StudentList);
            }
            catch (Exception ex)
            {
                return result.ExceptionResponse("Error retrieving student details.", ex);
            }
        }

        /// <summary>
        /// Retrieves user details for a specific user ID and checks that their status is not equal to 99.
        /// </summary>
        /// <param name="studentID">The ID of the user whose details are being retrieved.</param>
        /// <returns></returns>
        public ApiResult<studentDetailModel> GetStudentDetailsbyID(long studentID)
        {
            ApiResult<studentDetailModel> result = new ApiResult<studentDetailModel>();
            try
            {
                var StudentList = _context.studentDetails.Where(u => u.studentID == studentID && u.studentstatus != 99)
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
                        createBy = s.createBy,
                        modifiedBy = s.modifiedBy,
                        modifiedOn = s.modifiedOn,
                        studentPassword = s.studentPassword,
                        studentstatus = s.studentstatus
                    }).ToList();

                if (StudentList.Count > 0)
                {
                    return result.SuccessResponse("Success", StudentList);
                }
                return result.SuccessResponse("No data found", StudentList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add or update user type details
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<ApiResult<studentDetailModel>> AddorupdateStudentDetails(studentDetailModel role)
        {
            ApiResult<studentDetailModel> result = new ApiResult<studentDetailModel>();
            try
            {

                var existingStudent = _context.studentDetails.FirstOrDefault(a => a.studentID == role.studentID);
                if (existingStudent != null)
                {
                    // Update existing student details
                    existingStudent.firstName = role.firstName;
                    existingStudent.lastName = role.lastName;
                    existingStudent.email = role.email;
                    existingStudent.mobileNumber = role.mobileNumber;
                    existingStudent.gender = role.gender;
                    existingStudent.dateOfBirth = role.dateOfBirth;
                    existingStudent.modifiedBy = role.modifiedBy;
                    existingStudent.modifiedOn = DateTime.Now; // Track modification time
                    existingStudent.studentPassword = role.studentPassword;

                    _context.SaveChanges();
                    return result.SuccessResponse("Updated successfully.", role);
                }
                else
                {
                    // Create new student entry
                    var newStudent = new studentDetail
                    {
                        firstName = role.firstName,
                        lastName = role.lastName,
                        dateOfBirth = role.dateOfBirth,
                        gender = role.gender,
                        email = role.email,
                        mobileNumber = role.mobileNumber,
                        createdOn = DateTime.Now,
                        createBy = role.createBy,
                        modifiedOn = DateTime.Now,
                        modifiedBy = role.modifiedBy,
                        studentPassword = role.studentPassword,
                        studentstatus = role.studentstatus
                    };

                    _context.studentDetails.Add(newStudent);
                    _context.SaveChanges();
                    return result.SuccessResponse("Created successfully.", role);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete usertype by Id
        /// </summary>
        /// <param name="StudentID"></param>
        /// <returns></returns>
        public async Task<ApiResult<studentDetailModel>> DeleteStudentDetails(int id)
        {
            ApiResult<studentDetailModel> result = new ApiResult<studentDetailModel>();

            try
            {
                // Find the student by ID
                var student = await _context.studentDetails.FindAsync(id);

                if (student != null)
                {
                    // Remove the student from the context
                    _context.studentDetails.Remove(student);
                    await _context.SaveChangesAsync();

                    // Indicate success in the response
                    result.ResponseCode = 1;
                    result.Message = "Student deleted successfully.";
                }
                else
                {
                    // Handle case where the student is not found
                    result.ResponseCode = 0; // Indicate student not found
                    result.Message = "Student not found.";
                }
            }
            catch (Exception ex)
            {
                // Log the error (you might want to use a logging framework)
                result.ResponseCode = -1; // Indicate an error occurred
                result.Message = "Error while deleting student.";
                result.ErrorDesc = ex.Message; // Include error details
            }

            return result; // Ensure to return the result
        }

       

        //public async Task<studentDetail> GetByEmailAsync(string email)
        //{
        //    return await _context.studentDetails.FirstOrDefaultAsync(s => s.email == email);
        //}

        //public async Task UpdateAsync(studentDetail entity)
        //{
        //    _context.studentDetails.Update(entity);
        //    await _context.SaveChangesAsync();
        //}
    }

}
