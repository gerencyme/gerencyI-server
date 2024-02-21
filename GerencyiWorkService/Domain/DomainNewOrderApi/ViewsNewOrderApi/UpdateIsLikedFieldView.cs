using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainNewOrderApi.ViewsNewOrderApi
{
    public class UpdateIsLikedFieldView
    {
        public string OrderId { get; set; }
        public bool IsLiked { get; set; }
        public string CompanieCNPJ { get; set; }
    }
}
