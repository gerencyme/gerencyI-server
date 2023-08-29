using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Product
    {
        [Key]
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string DescriptionProduct { get; set; }

        public int Stock { get; set; }

        public int UnitPrice { get; set; }

        public string CompanyId { get; set; }

        public ICollection<DemandProduct> DemandProducts { get; set; } = new List<DemandProduct>(); // Inicialização para evitar null reference

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; } // Propriedade de navegação para Company
    }
}
