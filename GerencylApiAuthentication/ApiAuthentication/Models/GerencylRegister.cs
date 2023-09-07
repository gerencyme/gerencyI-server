using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiAuthentication.Models
{
    public class GerencylRegister : IdentityUser
    {

        public required string Email { get; set; }

        public required string Senha { get; set; }

        public required string CNPJ { get; set; }

        public required string PhantasyName { get; set; }

        public required string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}
