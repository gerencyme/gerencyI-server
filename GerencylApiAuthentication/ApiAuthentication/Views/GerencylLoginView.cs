using System.ComponentModel.DataAnnotations;

namespace ApiAuthentication.Views
{
    public class GerencylLoginView
    {

        [StringLength(50, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 8)]
        public required string Password { get; set; }

        [StringLength(50, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 8)]
        public required string CNPJ { get; set; }
    }
}
