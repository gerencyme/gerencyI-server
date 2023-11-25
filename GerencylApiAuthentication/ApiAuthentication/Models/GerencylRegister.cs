using Microsoft.AspNetCore.Identity;

namespace ApiAuthentication.Models
{
    public class GerencylRegister : IdentityUser
    {

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string CNPJ { get; set; }

        public required string PhantasyName { get; set; }

        public required string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}
