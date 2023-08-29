using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Enums;

namespace Entities
{
    public class Stand
    {
        [Key]
        public string StandId { get; set; }

        public string StandNumber { get; set; }

        public bool StandAllReadReserved { get; set; }

        public bool UserPermission { get; set; }

        public string CPFResponsible { get; set; }

        public float PaymentTotal { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }

        public OrderStatus StandStatus { get; set; }

        public string CompanyId { get; set; }

        public ICollection<Demand> Demands { get; set; } = new List<Demand>();

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; } // Propriedade de navegação para Company
    }
}
