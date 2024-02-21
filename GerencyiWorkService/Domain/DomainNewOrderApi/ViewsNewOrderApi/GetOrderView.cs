namespace Domain.DomainNewOrderApi.ViewsNewOrderApi
{
    [Serializable]
    public class GetOrderView : PaginationsControllersView
    {
        public string CompanieCNPJ { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
