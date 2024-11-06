//using Microsoft.AspNetCore.Mvc;
//using studentDetails_Api.Models;
//using studentDetails_Api.NonEntity;
//using studentDetails_Api.Services;
//using UsersDetailsApi.Models;

//[ApiController]
//[Route("api/[controller]")]
//public class UsersController : ControllerBase
//{
//    private readonly JwtServices _jwtServices;
//    private readonly StudentDBContext _context;

//    public UsersController(JwtServices jwtServices, StudentDBContext context)
//    {
//        _jwtServices = jwtServices;
//        _context = context;
//    }

//    [HttpPost("login")]
//    public IActionResult Login([FromBody] UserTypeMasterModel loginRequest)
//    {
//        var user = _context.UsersDetails
//            .FirstOrDefault(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password);

//        if (user == null)
//            return Unauthorized("Invalid credentials");

//        var token = _jwtServices.GenerateToken(user);
//        return Ok(new { Token = token });
//    }
//}
