namespace Entities.Entities
{
    [Serializable]
    public class DailyOrderTotal
    {
        public DateTime Date { get; set; }
        public double TotalOrders { get; set; }
    }
}
