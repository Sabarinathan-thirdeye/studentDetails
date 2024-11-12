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
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var jwtTokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]);

                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["JWT:Issuer"],
                        ValidAudience = _configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };

                    jwtTokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                    context.Items["User"] = validatedToken;
                }
                catch
                {
                    context.Response.StatusCode = 401;  // Unauthorized
                    await context.Response.WriteAsync("Unauthorized access");
                    return;
                }
            }

            await _next(context);
        }
    }
}
