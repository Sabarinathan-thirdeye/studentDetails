//using Microsoft.EntityFrameworkCore;
//using studentDetails_Api.IRepository;
//using studentDetails_Api.Models;
//using System.Threading.Tasks;

//namespace studentDetails_Api.Repository
//{
//    public class UserRepository : IUserRepository
//    {
//        private readonly StudentDBContext _context;

//        public UserRepository(StudentDBContext context)
//        {
//            _context = context;
//        }

//        public async Task<UsersDetail?> GetUserByEmailAndPasswordAsync(string email, string password)
//        {
//            // Fetch user based on email and password. If using hashed passwords, handle verification accordingly.
//            return await _context.UsersDetails.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
//        }
//    }
//}
