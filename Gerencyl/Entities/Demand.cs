using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Demand
    {
        [Key]
        public string DemandId { get; private set; }


        public string Observation { get; private set; }


        public DateTime DateDemand { get; private set; }

        public virtual Product Product { get; private set; }

        public string StandId { get; private set; }

        public ICollection<DemandProduct> DemandProducts { get; set; } = new List<DemandProduct>();

        [ForeignKey("StandId")]
        public virtual Stand Stand { get; set; }


        public Demand()
        {
            DateDemand = DateTime.Now;
        }

        public Demand(string demandId) : this()
        {
            DemandId = demandId;
        }

        public void AddDemand(string demandId, string observation, DateTime date, string standId)
        {
            DemandId = demandId;
            Observation = observation;
            DateDemand = date;
            StandId = standId;

        }

        public void AlterarTeste(string observation)
        {
            if (observation == null)
            {
                Observation = "";
            }
            Observation = observation;
        }

    }
}
