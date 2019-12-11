using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_Recarga.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Lembrar-me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O campo Email é obrigatório!")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Password é obrigatório!")]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Password")]
        [Compare("Password", ErrorMessage = "As passwords não coincidem.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório!")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo NIF é obrigatório!")]
        [Display(Name = "NIF")]
        [StringLength(9, ErrorMessage = "O NIF tem de ter 9 dígitos.", MinimumLength = 9)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "O NIF tem de ter 9 dígitos")]
        public string Nif { get; set; }

        [Required(ErrorMessage = "O campo Número do Cartão é obrigatório!")]
        [Display(Name = "Número do Cartão")]
        [StringLength(16, ErrorMessage = "O Número do Cartão tem de ter 16 dígitos.", MinimumLength = 16)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "O Número do CC tem de ter 16 dígitos")]
        public string NCartaoCredito { get; set; }

        [Required(ErrorMessage = "O campo Titular do Cartão é obrigatório!")]
        [Display(Name = "Titular do Cartão")]
        public string TitularCartao { get; set; }

        [Required(ErrorMessage = "O campo CCV é obrigatório!")]
        [Display(Name = "CCV")]
        [StringLength(3, ErrorMessage = "O CCV tem de ter 3 dígitos.", MinimumLength = 3)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "O CCV tem de ter 3 dígitos")]
        public string Ccv { get; set; }

        [Required(ErrorMessage = "O campo Tipo de Conta é obrigatório!")]
        [Display(Name = "Tipo de Conta")]
        public string Role { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
