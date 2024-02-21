using Entities;

namespace Domain.DomainNewOrderApi.ViewsNewOrderApi
{
    public class OrderCardView
    {
        public Guid OrderId { get; set; }
        public Guid CompanyId { get; set; }
        public string OrderColorIdentity { get; set; }
        public bool IsLiked { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public Product Product { get; set; }
        public List<PartnerCompaniesInthisOrderView> PartnerCompaniesInthisOrder { get; set; }
    }
}