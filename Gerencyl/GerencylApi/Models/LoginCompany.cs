using System.ComponentModel.DataAnnotations;

namespace GerencylApi.Models
{
    public class LoginCompany
    {
        [Required()]
        [EmailAddress()]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 8)]
        public string Senha { get; set; }

        [Required]
        public string CNPJ { get; set; }
    }
}
