using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entities
{
    [Table("DEMAND")]
    public class Demand
    {
        [Column("demand_id")]
        public string DemandId { get; private set; }

        [Column("observation")]
        public string Observation { get; private set; }

        [Column("date_demand")]
        public DateTime DateDemand { get; private set; }

        public virtual Product Product { get; private set; }

        public string ProductId { get; private set; }

        //public List<Product> Products { get; private set; } = new List<Product>();

        public Demand()
        {
                DateDemand = DateTime.Now;
        }

        public Demand (string demandId ) : this()
        {
            DemandId = demandId;
        }

        public void AddDemand(string demandId, string observation, DateTime date, string productId)
        {
            DemandId = demandId;
            Observation = observation;
            DateDemand = date;
            ProductId = productId;

        }

        public void AlterarTeste(string observation, string observation1, DateTime date, string productId)
        {
            if(observation == null)
            {
                Observation = "";
            }
            Observation = observation;
        }

    }
}


/*
 * var demand = new Demand();
 * demand.AlterarTeste("teste");
 * 
 */