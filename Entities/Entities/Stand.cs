using Entities.Enums;

namespace Entities.Entities
{
    public class Stand
    {
        public string StandId { get; set; }

        public string StandNumber { get; set; }

        public bool StandAllReadReserved { get; set; }

        public bool UserPermission { get; set; }

        public string CPFResponsible { get; set; }

        public float PaymentTotal { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }

        public OrderStatus StandStatus { get; set; }
    }
}