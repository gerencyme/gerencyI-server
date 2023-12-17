using MongoDB.Bson;

namespace Entities
{
    [Serializable]
    public class SimilarCompany
    {
        private ObjectId _id;
        private string _alt;
        private string _companyName;
        private string _src;

        public ObjectId Id { get => _id; private set => _id = value; }

        public string Alt { get => _alt; private set => _alt = value; }

        public string CompanyName { get => _companyName; private set => _companyName = value; }

        public string Src { get => _src; private set => _src = value; }
    }
}
