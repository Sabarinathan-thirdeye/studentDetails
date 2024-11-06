//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.DependencyInjection;
//using studentDetails_Api.Services;
//using System.Linq;
//using System.Threading.Tasks;

//namespace studentDetails_Api.Middleware
//{
//    /// <summary>
//    /// JWT Middleware
//    /// </summary>
//    public class JwtMiddleware
//    {
//        private readonly RequestDelegate _next;

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        /// <param name="next"></param>
//        public JwtMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        /// <summary>
//        /// Invoke Action
//        /// </summary>
//        /// <param name="context">App Context</param>
//        /// <returns></returns>
//        public async Task InvokeAsync(HttpContext context)
//        {
//            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
//            var token = authorizationHeader?.Replace("Bearer ", string.Empty);

//            if (!string.IsNullOrEmpty(token))
//            {
//                var jwtServices = context.RequestServices.GetService<JwtServices>();
//                var claimData = jwtServices.GetClaimData();

//                // If token is valid, set the claims in the context
//                if (claimData != null)
//                {
//                    context.Items["ClaimData"] = claimData;
//                }
//            }

//            await _next(context);
//        }
//    }
//}
