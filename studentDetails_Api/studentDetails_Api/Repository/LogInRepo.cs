//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using studentDetails_Api.Common.NonEntities;
//using studentDetails_Api.IRepository;
//using studentDetails_Api.Models;
//using studentDetails_Api.NonEntity;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using LogInResponse = studentDetails_Api.Models.LogInResponse;

//namespace studentDetails_Api.Repository
//{
//    public class LogInRepo : ILogInRepo
//    {
//        private readonly StudentDBContext _context;

//        public LogInRepo(StudentDBContext context)
//        {
//            _context = context;
//        }

//        public async Task<ApiResult<LogInModel>> Login(LogInModel request)
//        {
//            ApiResult<LogInModel> result = new ApiResult<LogInModel>();

//            var student = await _context.studentDetails
//                .FirstOrDefaultAsync(u => u.email == request.Email && u.studentPassword == request.Password);

//            if (student == null)
//            {
//                return result.ValidationErrorResponse("Invalid email or password.");
//            }

//            // Generate JWT token
//            var token = GenerateJwtToken(student);
//            LogInResponse logInResponse = new LogInResponse { Token = token };
//            return result.SuccessResponse("Login successful.", logInResponse);
//        }



//        private string GenerateJwtToken(studentDetail student)
//        {
//            // Replace this with your actual configuration values
//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SignSecRetK3y$End!nE@PikEy!nS3ckEy"));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//            var claims = new[]
//            {
//                new Claim(ClaimTypes.NameIdentifier, student.studentID.ToString()),
//                new Claim(ClaimTypes.Email, student.email),
//            };

//            var token = new JwtSecurityToken(
//                issuer: "studentDetails_Api",
//                audience: "studentDetails_Api",
//                claims: claims,
//                expires: DateTime.Now.AddMinutes(30),
//                signingCredentials: creds);

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}
