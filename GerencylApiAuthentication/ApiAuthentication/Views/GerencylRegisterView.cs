using ApiAuthentication.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiAuthentication.Views
{
    [Serializable]
    public class GerencylRegisterView
    {
        public string? Id { get; set; }

        [EmailAddress()]
        public required string Email { get; set; }

        [StringLength(20, MinimumLength = 18, ErrorMessage = "CNPJ cannot have less than 18 characters.")]
        public required string CNPJ { get; set; }

        [StringLength(100, MinimumLength = 2, ErrorMessage = "corporate cannot have less than 2 characters.")]
        public required string CorporateReason { get; set; }

        [StringLength(150, MinimumLength = 2, ErrorMessage = "Name cannot have less than 2 characters.")]
        public required string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
            ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número, um caractere especial e ter pelo menos 8 caracteres.")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
        public string? Password { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
            ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número, um caractere especial e ter pelo menos 8 caracteres.")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
        public string? ConfirmPassword { get; set; }

        //public PasswordView Password { get; set; }

        //public Cep cep { get; set; }
    }
}
