using Microsoft.AspNetCore.Identity;

namespace Entities.Entities
{

    public class Company : IdentityUser
    {

        public int CompanyId { get; set; }

        public string NameCompany { get; set; }

        public string cnpj { get; set; }

    }
}
