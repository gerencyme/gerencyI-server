using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiAuthentication.Views
{
    public class GerencylRegisterView
    {
        [EmailAddress()]
        public required string Email { get; set; }

        public required string CNPJ { get; set; }

        public required string PhantasyName { get; set; }

        public required string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public PasswordView? Password { get; set; }
    }
}
