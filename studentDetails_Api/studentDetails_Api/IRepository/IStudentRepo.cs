using studentDetails_Api.Models;
using studentDetails_Api.NonEntity;

namespace studentDetails_Api.IRepository
{
    public interface IStudentRepo
    {
        /// <summary>
        /// Retrive the All StudentDetails
        /// </summary>
        /// <returns></returns>
        ApiResult<studentDetailModel> GetStudentDetails();
        /// <summary>
        /// Retrive the All StudentDetails by UserID
        /// </summary>
        /// <param name="studentID"></param>
        /// <returns></returns>
        ApiResult<studentDetailModel> GetStudentDetailsbyID(long studentID);
        /// <summary>
        /// Addorupdate the StudentDetails 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<ApiResult<studentDetailModel>> AddorupdateStudentDetails(studentDetailModel role);
        /// <summary>
        /// Deleted the StudentDetails using Student Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<studentDetailModel>> DeleteStudentDetails(int id);
        
    }
}
