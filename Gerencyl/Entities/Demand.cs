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
    [Table("DEMAND")]
    public class Demand
    {
        
        [Column("DemandId")]
        public int DemandId { get; set; }

        [Column("observation")]
        public string Observation { get; set; }

        [Column("date_demand")]
        public DateTime DateDemand { get; set; }

       /* [ForeignKey("Stand")]
        public int StandId { get; set; }
        public virtual Stand Stand { get; set; }*/

        //public ICollection<DemandProduct> DemandProducts { get; set; } = new List<DemandProduct>();

    public Demand()
        {
            DateDemand = DateTime.Now;
        }

        public Demand(int demandId) : this()
        {
            DemandId = demandId;
        }

        public void AddDemand(int demandId, string observation, DateTime date)
        {
            DemandId = demandId;
            Observation = observation;
            DateDemand = date;
            //StandId = standId;

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