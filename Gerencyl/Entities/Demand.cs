using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Entities
{
    public class Demand
    {
        [BsonId]
        public ObjectId DemandId { get; set; }

        public string Observation { get; set; }

        public DateTime OpenDateDemand { get; set; }

        public DateTime ClosedDateDemand { get; set; }

        public ObjectId StandId { get; set; }

        public List<DemandProduct> DemandProducts { get; set; } = new List<DemandProduct>();

        public Demand()
        {
            DemandId = ObjectId.GenerateNewId();
            OpenDateDemand = DateTime.Now;
        }

        public void AddDemand(ObjectId standId, string observation, DateTime date)
        {
            StandId = standId;
            Observation = observation;
            OpenDateDemand = date;
            // Set other properties as needed.
        }

        public void AlterObservation(string observation)
        {
            Observation = observation ?? "";
        }
    }
}
