using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    [Table("DEMAND_PRODUCT")]
    public class DemandProduct
    {
        [Key]
        [Column("DemandProductId")]
        public int DemandProductId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

       
        public int DemandId { get; set; }
        public virtual Demand Demand { get; set; }

        
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
