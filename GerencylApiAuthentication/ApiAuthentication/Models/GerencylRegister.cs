using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiAuthentication.Models
{
    public class GerencylRegister : Register
    {
        [MaxLength(20)]
        public required string CNPJ { get; set; }

        [MaxLength(150)]
        public required string CorporateReason { get; set; }

    }
}
