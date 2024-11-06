using UsersDetailsApi.Models;
using UsersDetailsApi.IRepository;
using UsersDetailsApi.NonEntity;

namespace UsersDetailsApi.IRepository
{
    /// <summary>
    /// Repository for USER DETAILS operations.
    /// </summary>
    public class UserRepo : IUserRepo
    {
        private readonly UsersDBContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepo"/> class.
        /// </summary>
        public UserRepo(UsersDBContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Retrieves all user details 
        /// </summary>
        ApiResult<UserModel> IUserRepo.GetUserDetails()
        {
            ApiResult<UserModel> result = new ApiResult<UserModel>();
            try
            {
                //var claimData = _contextAccessor.HttpContext?.Items["ClaimData"] as ClaimData;
                //var varName = _context.dbname.condition
                var UserList = _context.Users.Where(u => u.UserTypeStatus != 99).Select(s => new UserModel
                {
                    UserID = s.UserID,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    CreatedOn = s.CreatedOn,
                    CreateBy = s.CreateBy,
                    ModifiedBy = s.ModifiedBy,
                    ModifiedOn = s.ModifiedOn,
                    UserTypeStatus = s.UserTypeStatus,
                    UserEmail = s.UserEmail,
                    UserName = s.UserName,
                    MobileNo = s.MobileNo,
                    Gender = s.Gender
                }).ToList();

                if (UserList.Count > 0)
                {
                    return result.SuccessResponse("Success", UserList);
                }
                return result.SuccessResponse("No data found", UserList);
            }
            catch (Exception ex)
            {
                return result.ExceptionResponse("Error retrieving user details.", ex);
            }
        }


        ///// <summary>
        ///// Retrieves user details for a specific user ID and checks that their status is not equal to 99.
        ///// </summary>
        //public ApiResult<UserModel> GetStudentDetailsbyID(long userId)
        //{
        //    ApiResult<UserModel> result = new ApiResult<UserModel>();
        //    try
        //    {
        //        var claimData = _contextAccessor.HttpContext?.Items["ClaimData"] as ClaimData;

        //        var studentDetails = _context.Users.Where(u => u.UserID == userId && u.UserTypeStatus != 99)
        //            .Select(s => new UserModel
        //            {
        //                UserID = s.UserID,
        //                FirstName = s.FirstName,
        //                LastName = s.LastName,
        //                CreatedOn = s.CreatedOn,
        //                CreateBy = s.CreateBy,
        //                ModifiedBy = s.ModifiedBy,
        //                ModifiedOn = s.ModifiedOn,
        //                UserTypeStatus = s.UserTypeStatus,
        //                UserEmail = s.UserEmail,
        //                UserName = s.UserName,
        //                MobileNo = s.MobileNo,
        //                Gender = s.Gender
        //            }).FirstOrDefault();

        //        if (studentDetails != null)
        //        {
        //            return result.SuccessResponse("Success", studentDetails);
        //        }
        //        return result.SuccessResponse("No data found", studentDetails);
        //    }
        //    catch (Exception ex)
        //    {
        //        return result.ExceptionResponse("Error retrieving user details.", ex);
        //    }
        //}

        ///// <summary>
        ///// Add or update user details.
        ///// </summary>
        //public async Task<ApiResult<UserModel>> AddorupdateStudentDetails(UserModel userModel)
        //{
        //    ApiResult<UserModel> result = new ApiResult<UserModel>();
        //    try
        //    {
        //        var claimData = _contextAccessor.HttpContext?.Items["ClaimData"] as ClaimData;

        //        var existingUser = _context.Users.FirstOrDefault(a => a.UserID == userModel.UserID);
        //        if (existingUser != null)
        //        {
        //            // Update existing user details
        //            existingUser.FirstName = userModel.FirstName;
        //            existingUser.LastName = userModel.LastName;
        //            existingUser.UserEmail = userModel.UserEmail;
        //            existingUser.MobileNo = userModel.MobileNo;
        //            existingUser.Gender = userModel.Gender;
        //            existingUser.ModifiedBy = userModel.ModifiedBy;
        //            existingUser.ModifiedOn = DateTime.Now;

        //            await _context.SaveChangesAsync();
        //            return result.SuccessResponse("Updated successfully.", userModel);
        //        }
        //        else
        //        {
        //            // Create new user entry
        //            var newUser = new UserModel
        //            {
        //                FirstName = userModel.FirstName,
        //                LastName = userModel.LastName,
        //                UserEmail = userModel.UserEmail,
        //                MobileNo = userModel.MobileNo,
        //                Gender = userModel.Gender,
        //                CreatedOn = DateTime.Now,
        //                CreateBy = userModel.CreateBy,
        //                ModifiedOn = DateTime.Now,
        //                ModifiedBy = userModel.ModifiedBy,
        //                UserTypeStatus = userModel.UserTypeStatus
        //            };

        //            _context.Users.Add(newUser);
        //            await _context.SaveChangesAsync();
        //            return result.SuccessResponse("Created successfully.", userModel);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return result.ExceptionResponse("Error adding or updating user details.", ex);
        //    }
        //}
    }
}