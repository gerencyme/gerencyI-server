using MongoDB.Bson;

namespace Entities
{
    [Serializable]
    public class Purchase
    {

        private readonly List<SimilarCompany> _similarCompaniesContent;

        private ObjectId _purchaseId;
        private DateTime _date;
        private string _identifyColor;
        private string _productBrand;
        private string _productName;
        private int _quantity;
        private string _status;
        private decimal _totalPrice;
        private decimal _unitPrice;
        private bool _isLiked;

        public Purchase()
        {
            _similarCompaniesContent = new List<SimilarCompany>();
        }

        public Purchase(ObjectId purchaseId, DateTime date, string identifyColor, string productBrand,
                       string productName, int quantity, string status, decimal totalPrice,
                       decimal unitPrice, bool isLiked)
            : this()
        {
            PurchaseId = purchaseId;
            Date = date;
            IdentifyColor = identifyColor;
            ProductBrand = productBrand;
            ProductName = productName;
            Quantity = quantity;
            Status = status;
            TotalPrice = totalPrice;
            UnitPrice = unitPrice;
            IsLiked = isLiked;
        }

        public ObjectId PurchaseId { get => _purchaseId; private set => _purchaseId = value; }

        public DateTime Date { get => _date; private set => _date = value; }

        public string ProductBrand { get => _productBrand; private set => _productBrand = value; }

        public string ProductName { get => _productName; private set => _productName = value; }

        public int Quantity { get => _quantity; set => _quantity = value; }// Adicione a lógica de validação aqui, se necessário}

        public string Status { get => _status; private set => _status = value; }

        public decimal TotalPrice { get => _totalPrice; private set => _totalPrice = value; }

        public decimal UnitPrice { get => _unitPrice; private set => _unitPrice = value; }

        public bool IsLiked { get => _isLiked; private set => _isLiked = value; }

        public string IdentifyColor { get => _identifyColor; private set => _identifyColor = value; }

        public IReadOnlyList<SimilarCompany> SimilarCompaniesContent => _similarCompaniesContent.AsReadOnly();

        public void AddSimilarCompany(SimilarCompany similarCompany)
        {
            _similarCompaniesContent.Add(similarCompany);
        }

        private void CalculateTotalPrice()
        {
            TotalPrice = Quantity * UnitPrice;
        }
    }
}