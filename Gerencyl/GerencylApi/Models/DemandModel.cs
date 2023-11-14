using MongoDB.Bson;

namespace GerencylApi.Models
{
    public class DemandModel
    {
        public ObjectId DemandId { get; set; }

        public string Observation { get; set; }

        //public int ProductId { get; set; }

        public DateTime DateDemand { get; set; }
    }
}
