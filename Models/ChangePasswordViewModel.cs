using System.ComponentModel.DataAnnotations;

namespace WebNC_BTL_QLCV.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Mật khẩu hiện tại không được bỏ trống.")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới không được bỏ trống.")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới phải có ít nhất 6 ký tự.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu mới.")]
        [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }
    }
}
