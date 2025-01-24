using System.ComponentModel.DataAnnotations;

namespace TelephoneDirectory.MVC.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Eski şifre gereklidir")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre gereklidir")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre tekrarı gereklidir")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Yeni şifre ve tekrarı eşleşmiyor.")]
        public string ConfirmNewPassword { get; set; }
    }
}
