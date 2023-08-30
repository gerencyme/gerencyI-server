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
    [Table("STAND")]
    public class Stand
    {
        [Key]
        [Column("StandId")]
        public int StandId { get; set; }

        [Column("stand_number")]
        public string StandNumber { get; set; }

        [Column("stand_all_read_reserved")]
        public bool StandAllReadReserved { get; set; }

        [Column("user_permission")]
        public bool UserPermission { get; set; }

        [Column("cpf_responsible")]
        public string CPFResponsible { get; set; }

        [Column("payment_total")]
        public float PaymentTotal { get; set; }

        [Column("date_creation")]
        public DateTime DateCreation { get; set; }

        [Column("date_modification")]
        public DateTime DateModification { get; set; }

        public OrderStatus StandStatus { get; set; }

        [ForeignKey("Company")]
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public ICollection<Demand> Demands { get; set; } = new List<Demand>();
    }

}
