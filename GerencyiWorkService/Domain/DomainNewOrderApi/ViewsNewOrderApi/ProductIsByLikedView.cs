using Entities;

namespace Domain.DomainNewOrderApi.ViewsNewOrderApi
{
    public class ProductIsByLikedView
    {
        public Guid OrderId { get; set; }
        public Guid CompanyId { get; set; }
        public string OrderColorIdentity { get; set; }
        public string ProductName { get; set; }
        public string ProductBrand { get; set; }
        public string ProductType { get; set; }
        public int CountStatusUnderAnalise { get; set; }
        public int CountStatusDone { get; set; }
        public int CountStatusCancel { get; set; }
    }
}