using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsSender.Helpers
{
    public static class PhoneHelper
    {
        public static string PhoneCustomizer(string phone)
        {
            phone = phone.Replace("+", string.Empty).Replace(" ", string.Empty);
            if (phone.Length == 10)
            {
                phone = "90" + phone;
            }
            else if (phone.Length == 11)
            {
                phone = "9" + phone;
            }
            return phone;
        }
    }
}
