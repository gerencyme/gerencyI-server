using MongoDB.Bson;

namespace ApiAuthentication.Models
{
    [Serializable]
    public class Supplier
    {
        private Guid _supplierId;
        private string _nome;
        private string _cnpj;
        private string _endereco;
        private string _email;
        private string _telephone;

        public Supplier()
        {
            _supplierId = Guid.NewGuid();
        }

        public Guid SupplierId
        {
            get { return _supplierId; }
            set { _supplierId = value; }
        }

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public string Cnpj
        {
            get { return _cnpj; }
            set { _cnpj = value; }
        }

        public string Endereco
        {
            get { return _endereco; }
            set { _endereco = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Telephone
        {
            get { return _telephone; }
            set { _telephone = value; }
        }

    }
}
