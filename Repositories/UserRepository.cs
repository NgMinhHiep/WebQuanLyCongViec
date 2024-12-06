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
            return user != null && user.PassWord == password;
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
