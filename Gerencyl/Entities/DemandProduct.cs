using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class DemandProduct
    {
        public string Id { get; set; }
        public string DemandId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("Company")]
        public string CompanyId { get; set; }
        public Demand Demand { get; set; }
        public Product Product { get; set; }
    }
}
