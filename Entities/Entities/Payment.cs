using Entities.Enums.PaymentEnums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entities
{
    [Table("PAYMENT")]
    public class Payment
    {
        [Column("payment_id")]
        public int PaymentId { get; set; }

        [Column("stand_id")]
        public int StandId { get; set; }

        public PaymentEnums PaymentOrder { get; set; }

        public PaymentEnums PaymentMethod { get; set; }
    }
}
