using System.ComponentModel.DataAnnotations;

namespace ApiAuthentication.Models
{
    public class GerencylClientRegister
    {

        public string Email { get; set; }

        [MaxLength(20)]
        public required string CPF { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
