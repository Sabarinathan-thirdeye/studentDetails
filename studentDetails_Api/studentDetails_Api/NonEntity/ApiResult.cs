//using studentDetails_Api.Common.NonEntities;
using studentDetails_Api.Models;

namespace studentDetails_Api.NonEntity
{
    public class ApiResult<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApiResult()
        {
            ResponseCode = -1;
            Message = string.Empty;
            ErrorDesc = string.Empty;
            ResponseData = new List<T>();
        }
        /// <summary>
        /// Response Code
        /// 0 - for Exception
        /// 1 - for Success response
        /// 2 - for Validation errors
        /// </summary>                                                                                                             
        public int ResponseCode { get; set; }
        /// <summary>
        /// Response Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Error Description/Details. Usually the exception trace.
        /// </summary>
        public string ErrorDesc { get; set; }
        /// <summary>
        /// Response Data
        /// </summary>
        public List<T> ResponseData { get; set; }

    }

    public static class ApiResultExtenstions
    {
        public static ApiResult<T> ExceptionResponse<T>(this ApiResult<T> result, string message, Exception ex)
        {
            result.ResponseCode = 0;
            result.Message = message;
            result.ErrorDesc = ex.ToString();
            return result;
        }
        public static ApiResult<T> ValidationErrorResponse<T>(this ApiResult<T> result, string message)
        {
            result.ResponseCode = 2;
            result.Message = message;
            return result;
        }
        public static ApiResult<T> SuccessResponse<T>(this ApiResult<T> result, string message, List<T>? data)
        {
            result.ResponseCode = 1;
            result.Message = message;
            if (data == null || data.Count == 0)
            {
                result.Message = "No records found.";
            }
            result.ResponseData = data ?? new List<T>();
            return result;
        }
        public static ApiResult<T> SuccessResponse<T>(this ApiResult<T> result, string message, T data)
        {
            result.ResponseCode = 1;
            result.Message = message;
            result.ResponseData.Add(data);
            return result;
        }
    }
}