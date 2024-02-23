using System.ComponentModel.DataAnnotations;

namespace EdManagementSystem.App.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле 'Email' обязательно для заполнения.")]
        [EmailAddress(ErrorMessage = "Некорректный формат email адреса.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле 'Пароль' обязательно для заполнения.")]
        public string Password { get; set; }
    }
}
