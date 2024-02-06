using System.ComponentModel.DataAnnotations;
namespace TGM.Models.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Введен неверный логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введен неверный email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введен неверный пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
