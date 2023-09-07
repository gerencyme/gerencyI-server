using System.ComponentModel.DataAnnotations;

namespace ApiAuthentication.Views
{
    public class GerencylRegisterView
    {
        [EmailAddress()]
        public required string Email { get; set; }

        [StringLength(50, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 8)]
        public required string Senha { get; set; }

        [StringLength(50, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 8)]
        public required string ConfirmSenha { get; set; }
        [StringLength(16, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 16)]
        public required string CNPJ { get; set; }

        [StringLength(50, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public required string PhantasyName { get; set; }

        [StringLength(50, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public required string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
