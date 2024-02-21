using MongoDB.Bson;

namespace Domain.DomainNewOrderApi.ViewsNewOrderApi
{
    [Serializable]
    public class NewOrderUpdateView : NewOrderAddView
    {
        public Guid OrderId { get; set; }
    }
}
