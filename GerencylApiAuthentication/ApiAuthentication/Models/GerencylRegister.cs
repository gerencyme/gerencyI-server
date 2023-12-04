namespace ApiAuthentication.Models
{
    public class GerencylRegister : Register
    { 

        private string _cnpj;
        private string? _corporateReason;

        public GerencylRegister()
        {
        }

        /*public GerencylRegister(string email, string password, string name, string cnpj, string corporateReason, DateTime creationDate, DateTime updateDate)
        {
            Email = email;
            Password = password;
            Name = name;
            CreationDate = creationDate;
            UpdateDate = updateDate;
            CNPJ = cnpj;
            CorporateReason = corporateReason;
        }*/

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
