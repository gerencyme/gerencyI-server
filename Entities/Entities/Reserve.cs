using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    public class Reserve
    {
        public int ReserveId { get; set; }

        public DateTime ReservedDate { get; set; }

        public string CPF { get; set; }

        public string StandQuantityReserved { get; set; }


        /// <summary>
        /// abaixo configurar a fk
        /// </summary>
        public int StandId { get; set; }
    }
}
