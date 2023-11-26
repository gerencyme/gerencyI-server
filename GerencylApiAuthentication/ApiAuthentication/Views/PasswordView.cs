using System.ComponentModel.DataAnnotations;

namespace ApiAuthentication.Views
{
    public class PasswordView
    {
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
            ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número, um caractere especial e ter pelo menos 8 caracteres.")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
        public string? Pawssord { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
            ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número, um caractere especial e ter pelo menos 8 caracteres.")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
        public string? ConfirmPassword { get; set; }
    }
}
