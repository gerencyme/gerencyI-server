using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    [Table("COMPANY")]
    public class Company : IdentityUser
    {
        
        [Column("CompanyId")]
        public string CompanyId { get; set; }

        [Column("name_company")]
        public string NameCompany { get; set; }

        [Column("cnpj")]
        public string CNPJ { get; set; }

        //public virtual ICollection<Stand> Stands { get; set; } = new List<Stand>();
        public virtual ICollection<Product> Product { get; set; } = new List<Product>();
    }

}
