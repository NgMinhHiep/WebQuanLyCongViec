using WebNC_BTL_QLCV.Models;
namespace WebNC_BTL_QLCV.Repositories
{
    public interface IUserRepository
    {
        bool IsUserNameExist(string username);
        bool IsEmailExist(string email);
        bool IsPhoneNumberExist(string phonenumber);
        bool IsPasswordCorrect(string username, string password);

        IEnumerable<User> GetAllUsers();

        // lay ra user theo id
        User GetUserById(int id);

        // lay ra user theo tai khoan
        User GetUserByName(string username);

        // Them user
        void Add(User user);

        //Thống kê số lượng người dùng đã đăng kí
        int GetUserCountInCurrentMonth();
        int GetUserCountInCurrentWeek();
    }
}
