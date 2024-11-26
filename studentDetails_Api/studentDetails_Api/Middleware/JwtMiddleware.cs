using Microsoft.IdentityModel.Tokens;
using studentDetails_Api.NonEntity;
using studentDetails_Api.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace studentDetails_Api.Middleware
{
    /// <summary>
    /// JWT Middleware
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next"></param>
        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }
        /// <summary>
        /// Invoke Action
        /// </summary>
        /// <param name="context">App Context</param>
        /// <param name="jwtServices">JWT Service</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, JwtServices jwtServices)
        {
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            var token = authorizationHeader?.Replace("Bearer ", string.Empty);

            if (!string.IsNullOrEmpty(token))
            {
                ClaimData claimData = jwtServices.GetClaimData();
                context.Items["ClaimData"] = claimData;
            }

            await _next(context);
        }
    }
}
