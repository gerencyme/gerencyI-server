using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiAuthentication.Models
{
    public class GerencylRegister : Register
    { 

        private string _cnpj;
        private string? _corporateReason;

        /*public GerencylRegister(string email, string password, string name, string cnpj, string corporateReason)
        {
            Email = email;
            Password = password;
            Name = name;
            CNPJ = cnpj;
            CorporateReason = corporateReason;
        }*/

        public GerencylRegister(string email, string password, string name, string cnpj, string corporateReason, DateTime creationDate, DateTime updateDate)
        : base(email,
               password,
               name,
               creationDate,
               updateDate)
        {
            CNPJ = cnpj;
            CorporateReason = corporateReason;
        }

        public string CNPJ
        {
            get { return _cnpj; }
            private set { _cnpj = value; }
        }

        public string CorporateReason
        {
            get { return _corporateReason; }
            private set { _corporateReason = value; }
        }

    }
}
