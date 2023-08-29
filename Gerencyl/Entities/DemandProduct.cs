using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class DemandProduct
    {
        [Key]
        [MaxLength(191)]
        public string DemandProductId { get; set; }

        [Required]
        [MaxLength(191)]
        public string DemandId { get; set; }

        [Required]
        [MaxLength(191)]
        public string ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [ForeignKey("DemandId")] // Chave estrangeira correta
        public virtual Demand Demand { get; set; }

        [ForeignKey("ProductId")] // Chave estrangeira correta
        public virtual Product Product { get; set; }
    }
}
