//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.DependencyInjection;
//using studentDetails_Api.Services;
//using System.Linq;
//using System.Threading.Tasks;

//namespace studentDetails_Api.Middleware
//{
//    public class JwtMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public JwtMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
//            var token = authorizationHeader?.Replace("Bearer ", string.Empty);

//            if (!string.IsNullOrEmpty(token))
//            {
//                var jwtServices = context.RequestServices.GetService<JwtServices>();
//                var claimData = jwtServices.GetClaimData();

//                if (claimData != null)
//                {
//                    context.Items["ClaimData"] = claimData;
//                }
//            }

//            await _next(context);
//        }
//    }
//}
