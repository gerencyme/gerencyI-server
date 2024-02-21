using Entities.Entities;
using Entities;
using MongoDB.Bson;

namespace Domain.DomainNewOrderApi.ViewsNewOrderApi
{
    [Serializable]
    public class NewOrderAddView
    {
        public Guid CompanyId { get; set; }
        public string CompanieCNPJ { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderColorIdentity { get; set; }
        public Product Product { get; set; }
        public Location Location { get; set; }
    }
}
