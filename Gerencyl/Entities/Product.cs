using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    [Table("PRODUCT")]
    public class Product
    {
        [Key]
        [Column("ProductId")]
        public int ProductId { get; set; }

        [Column("product_name")]
        public string ProductName { get; set; }

        [Column("description_product")]
        public string DescriptionProduct { get; set; }

        [Column("stock")]
        public int Stock { get; set; }

        [Column("unit_price")]
        public int UnitPrice { get; set; }

        [ForeignKey("Company")]
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public ICollection<DemandProduct> DemandProducts { get; set; } = new List<DemandProduct>();
    }
}
