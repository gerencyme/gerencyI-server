﻿using AspNetCore.Identity.MongoDbCore.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiAuthentication.Models
{
    [Serializable]
    public class Register : MongoIdentityUser<Guid>
    {
        private string _password;
        private string _name;
        //private byte[] _userImg;
        private DateTime _creationDate;
        private DateTime _updateDate;


        /* public byte[] UserImg
         {
             get { return _userImg; }
             set { _userImg = value; }
         }*/

        public string? RefreshToken { get; set; }
        public string? Token { get; set; }

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
