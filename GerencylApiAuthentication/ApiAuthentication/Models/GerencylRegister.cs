namespace ApiAuthentication.Models
{
    [Serializable]
    public class GerencylRegister : Register
    { 

        private string _cnpj;
        private string? _corporateReason;

        private ZipCode _zipCode { get; set; } = new ZipCode();
        public GerencylRegister()
        {
            // Lógica de inicialização, se necessário
        }

        public GerencylRegister(string cnpj, string corporateReason, ZipCode zipCode)
        {
            _cnpj = cnpj;
            _corporateReason = corporateReason;
            _zipCode = zipCode;
        }


        public ZipCode ZipCode
        {
            get { return _zipCode; }
            set { _zipCode = value; }
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

    }
}
