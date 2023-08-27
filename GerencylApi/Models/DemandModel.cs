using System.ComponentModel.DataAnnotations.Schema;

namespace GerencylApi.Models
{
    public class DemandModel
    {
        
        public string DemandId { get; set; }

        public string Observation { get; set; }

        public string ProductId { get; set; }

        public DateTime DateDemand { get; set; }
    }
}
