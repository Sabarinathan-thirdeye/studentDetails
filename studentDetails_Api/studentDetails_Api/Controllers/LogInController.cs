
//        using Microsoft.AspNetCore.Mvc;
//using studentDetails_Api.IRepository;
//using studentDetails_Api.Models;
//        using studentDetails_Api.NonEntity;
//        using studentDetails_Api.Services;

//namespace studentDetails_Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        private readonly IStudentRepo<studentDetail> _studentRepository; // Assuming IRepository is set up
//        private readonly JwtServices _jwtServices;
//        private readonly CryptoServices _cryptoServices;

//        public AuthController(IStudentRepo<studentDetail> studentRepository, JwtServices jwtServices, CryptoServices cryptoServices)
//        {
//            _studentRepository = studentRepository;
//            _jwtServices = jwtServices;
//            _cryptoServices = cryptoServices;
//        }

//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] LogInRequest loginRequest)
//        {
//            // Validate input
//            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
//            {
//                return BadRequest("Email and Password are required.");
//            }

//            // Fetch the user by email
//            var student = await _studentRepository.GetByEmailAsync(loginRequest.Email); // Assuming you have a method to get user by email

//            // If student not found or password does not match
//            if (student == null || student.studentPassword != _cryptoServices.DecryptStringFromBytes_Aes(loginRequest.Password))
//            {
//                return Unauthorized("Invalid email or password.");
//            }

//            // Generate JWT token
//            var token = _jwtServices.GenerateToken(student);

//            // Optionally, save the token in the database (encrypted)
//            student.StudentToken = _cryptoServices.EncryptStringToBytes_Aes(token);
//            await _studentRepository.UpdateAsync(student); // Assuming you have an Update method

//            // Return token
//            return Ok(new { Token = token });
//        }
//    }
//}
