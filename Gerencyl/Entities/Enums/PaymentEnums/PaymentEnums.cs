using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Enums.PaymentEnums
{
    public enum PaymentEnums
    {
        CreditCard = 1,
        DebitCard = 2,
        Pix = 3,
    }
    public enum PaymentOrder
    {
        WaitingPayment = 1,
        Concluded = 2

    }
}
