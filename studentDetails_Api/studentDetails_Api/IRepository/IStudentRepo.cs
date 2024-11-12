﻿using studentDetails_Api.Models;
using studentDetails_Api.NonEntity;

namespace studentDetails_Api.IRepository
{
    public interface IStudentRepo
    {
        /// <summary>
        /// Retrive the All active StudentDetails
        /// </summary>
        /// <returns></returns>
        ApiResult<studentDetailModel> GetStudentDetails();
        /// <summary>
        /// Retrive the Inactive StudentDetails
        /// </summary>
        /// <returns></returns>
         ApiResult<studentDetailModel> GetStudentDetailsInActive();
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
        /// edit student details
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        //Task<ApiResult<studentDetailModel>> EditCompanyDetails(studentDetailModel student);
        /// <summary>
        /// Updates the student status to inactive using Student ID.
        /// </summary>
        /// <param name="studentID"></param>
        /// <returns></returns>
        Task<ApiResult<studentDetailModel>> UpdateStudentStatusAsync(long studentID);
        
    }
}
