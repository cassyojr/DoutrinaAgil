using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DoutrinaAgil.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        [MinLength(3, ErrorMessage = "Informe no mínimo {0} caracteres")]
        [MaxLength(200, ErrorMessage = "Informe no máximo {0} caracteres")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Informe somente letras")]
        [DisplayName("Nome")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Senha")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "O campo senha não está igual")]
        [DisplayName("Confirmar senha")]
        public string ConfirmPassword { get; set; }
    }
}