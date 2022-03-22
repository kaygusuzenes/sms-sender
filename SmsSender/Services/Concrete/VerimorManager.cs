using SmsSender.Helpers;
using SmsSender.Services.Abstract;
using System.Collections.Generic;
using System.Configuration;

namespace SmsSender.Services.Concrete
{
    internal class VerimorManager : ISmsService
    {
        public string Sender(string phone, string message)
        {
            phone = PhoneHelper.PhoneCustomizer(phone);
            string istekAdresi = "http://sms.verimor.com.tr/v2/send.json";
            var Sms = new
            {
                username = ConfigurationManager.AppSettings["username"],
                password = ConfigurationManager.AppSettings["password"],
                source_addr = ConfigurationManager.AppSettings["orginator"],
                valid_for = "24:00",
                datacoding = "1",
                messages = new List<object>
                {
                    new
                    {
                       msg = message,
                       dest = phone
                    }
                }
            };
            return Statik.CreateWebRequest(istekAdresi, Sms.ToJsonSerialize(), "POST", "application/json");
        }
    }
}
