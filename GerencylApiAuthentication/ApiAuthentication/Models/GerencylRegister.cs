namespace ApiAuthentication.Models
{
    [Serializable]
    public class GerencylRegister : Register
    { 

        private string _cnpj;
        private string? _corporateReason;
        private string? _telephone;
        private string _companyImg;

        private ZipCode _zipCode { get; set; } = new ZipCode();

        private Supplier _supplier { get; set; } = new Supplier();
        public GerencylRegister()
        {
            // Lógica de inicialização, se necessário
        }

        public GerencylRegister(string cnpj, string corporateReason, ZipCode zipCode, Supplier supplier)
        {
            _cnpj = cnpj;
            _corporateReason = corporateReason;
            _zipCode = zipCode;
            _supplier = supplier;
        }


        public ZipCode ZipCode
        {
            get { return _zipCode; }
            set { _zipCode = value; }
        }

        public Supplier Supplier
        {
            get { return _supplier; }
            set { _supplier = value; }
        }

        public string Telephone
        {
            get { return _telephone; }
            set { _telephone = value; }
        }
        public string CNPJ
        {
            get { return _cnpj; }
            set { _cnpj = value; }
        }

        public string CorporateReason
        {
            get { return _corporateReason; }
            set { _corporateReason = value; }
        }

        public string CompanyImg
        {
            get { return _companyImg; }
            set { _companyImg = value; }
        }
    }
}
