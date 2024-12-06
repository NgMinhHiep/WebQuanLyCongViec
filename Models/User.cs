using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNC_BTL_QLCV.Models
{
    [Table("tblUser")]
    public class User
    {
        // ID người dùng
        [Key]
        [Column("iUserID")]
        public int UserID { get; set; }

        // Tên của người dùng
        [StringLength(100, ErrorMessage = "Họ tên không được quá 100 ký tự.")]
        [Required(ErrorMessage = "Họ tên không được bỏ trống.")]
        [Column("sFullName")]
        public string FullName { get; set; }

        // Ngày sinh của người dùng
        [Column("dDateOfBirth")]
        [Required(ErrorMessage = "Ngày sinh không được bỏ trống.")]
        public DateOnly DateOfBirth { get; set; }

        //Số điện thoại người dùng
        [StringLength(12, ErrorMessage = "Số điện thoại không được quá 12 ký tự.")]
        [Required(ErrorMessage = "Số điện thoại không được bỏ trống.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Số điện thoại chỉ được chứa các số.")]
        [Column("sPhoneNumber")]
        public string PhoneNumber { get; set; }

        // Email người dùng
        [Required(ErrorMessage = "Email không được bỏ trống.")]
        [StringLength(1000, ErrorMessage = "Email không được quá 1000 ký tự.")]
        [Column("sEmail")]
        public string Email { get; set; }

        // Tên tài khoản của người dùng
        [Required(ErrorMessage = "Tên tài khoản không được bỏ trống.")]
        [StringLength(70, ErrorMessage = "Tên tài khoản không được quá 70 ký tự.")]
        [Column("sUserName")]
        public string UserName { get; set; }

        // Mật khẩu của người dùng
        [Required(ErrorMessage = "Mật khẩu không được bỏ trống.")]
        [StringLength(256, ErrorMessage = "Mật khẩu không được quá 256 ký tự.")]
        [Column("sPassWord")]
        public string PassWord { get; set; }

        //Ngày tạo tài khoản
        [Column("dUser_CreationDate")]
        public DateTime User_CreationDate { get; set; }

        public ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();

        // Nhập lại mật khẩu
        /*
        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu.")]
        [Compare("PassWord", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }
        */
        /*
         [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Mật khẩu phải có đúng 10 ký tự.")]
    [RegularExpression(@"^[0-9][A-Za-z0-9]*$", ErrorMessage = "Mật khẩu phải bắt đầu bằng một số và có đúng 10 ký tự.")]
    public string Password { get; set; }

        <span asp-validation-for="Password" class="text-danger"></span> <!-- Hiển thị lỗi -->
         */
    }
}
