using System.ComponentModel.DataAnnotations;

namespace ApiAuthentication.Views
{
    public class GerencylRegisterView
    {
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

        public PasswordView Password { get; set; }
    }
}
