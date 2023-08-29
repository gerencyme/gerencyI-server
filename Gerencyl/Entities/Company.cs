using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Company : IdentityUser
    {

        [Key]
        public string CompanyId { get; set; }

        public string NameCompany { get; set; }
        public string cnpj { get; set; }

        public ICollection<Stand> Stands { get; set; } = new List<Stand>();
        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
