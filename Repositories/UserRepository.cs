using Microsoft.AspNetCore.Identity;
using WebNC_BTL_QLCV.Data;
using WebNC_BTL_QLCV.Models;

namespace WebNC_BTL_QLCV.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsUserNameExist(string username)
        {
            return _context.Users.Any(u => u.UserName == username);
        }

        public bool IsEmailExist(string email) 
        { 
            return _context.Users.Any(u => u.Email == email);
        }

        public bool IsPhoneNumberExist(string phonenumber)
        {
            return _context.Users.Any(u => u.PhoneNumber == phonenumber);
        }

        public bool IsPasswordCorrect(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == username);

            var passwordHasher = new PasswordHasher<User>();
            // Xác minh mật khẩu
            var result = passwordHasher.VerifyHashedPassword(user, user.PassWord, password);

            return result == PasswordVerificationResult.Success;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public User GetUserByName(string username)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == username);
        }

        public void Add(User user)
        {
            _context.Users.Add(user); // them nguoi dung moi
            _context.SaveChanges(); // luu vao csdl
        }

        public void Update(User user)
        {
            var existingUser = GetUserById(user.UserID);

            // cập nhật các trường cần thiết
            existingUser.FullName = user.FullName;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.Email = user.Email;

            // đánh dấu trường cập nhật
            _context.Entry(existingUser).Property(x => x.FullName).IsModified = true;
            _context.Entry(existingUser).Property(x => x.PhoneNumber).IsModified = true;
            _context.Entry(existingUser).Property(x => x.Email).IsModified = true;
            _context.Entry(existingUser).Property(x => x.DateOfBirth).IsModified = true;

            // các trường không cập nhật
            _context.Entry(existingUser).Property(x => x.UserID).IsModified = false;
            _context.Entry(existingUser).Property(x => x.UserName).IsModified = false;
            _context.Entry(existingUser).Property(x => x.PassWord).IsModified = false;
            _context.Entry(existingUser).Property(x => x.RoleID).IsModified = false;

            _context.SaveChanges();
        }

        public int GetUserCountInCurrentMonth()
        {
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            return _context.Users.Count(u => u.User_CreationDate >= startOfMonth);
        }

        public int GetUserCountInCurrentWeek()
        {
            var startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            return _context.Users.Count(u => u.User_CreationDate >= startOfWeek);
        }
    }
}
