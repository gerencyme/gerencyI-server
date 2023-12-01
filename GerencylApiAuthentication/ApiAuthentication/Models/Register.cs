using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiAuthentication.Models
{
    public class Register : IdentityUser
    {
        private string _email;
        private string _password;
        private string _name;
        private DateTime _creationDate;
        private DateTime _updateDate;

        public Register(string userName, string password, string name, DateTime creationDate, DateTime updateDate) : base(userName)
        {
            _password = password;
            _name = name;
            _creationDate = creationDate;
            _updateDate = updateDate;
            _name = name;
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        [MaxLength(30)]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        [MaxLength(150)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }

        public DateTime UpdateDate
        {
            get { return _updateDate; }
            set { _updateDate = value; }
        }
    }
}
