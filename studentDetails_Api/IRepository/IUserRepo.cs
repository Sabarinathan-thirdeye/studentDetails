using UsersDetailsApi.NonEntity;
using System.Threading.Tasks;
using UsersDetailsApi.Models;

namespace UsersDetailsApi.IRepository
{
    public interface IUserRepo
    {
        /// <summary>
        /// Retrieves all user details.
        /// </summary>
        ApiResult<UserModel> GetUserDetails();

        ///// <summary>
        ///// Retrieves user details by user ID.
        ///// </summary>
        ///// <param name="userId">The ID of the user to retrieve details for.</param>
        //ApiResult<UserModel> GetUserDetailsByID(long userId);

        ///// <summary>
        ///// Adds or updates user details.
        ///// </summary>
        ///// <param name="user">The user details to add or update.</param>
        //Task<ApiResult<UserModel>> AddOrUpdateUserDetails(UserModel user);

        ///// <summary>
        ///// Deletes user details using user ID.
        ///// </summary>
        ///// <param name="userId">The ID of the user to delete.</param>
        //Task<ApiResult<UserModel>> DeleteUserDetails(long userId);
    }
}