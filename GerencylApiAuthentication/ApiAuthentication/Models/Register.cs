using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiAuthentication.Models
{
    public class Register : IdentityUser
    {

        public required string Email { get; set; }

        [MaxLength(30)]
        public required string Password { get; set; }

        [MaxLength(150)]
        public required string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}
