using SmsSender.Helpers;
using SmsSender.Services.Abstract;
using System.Collections.Generic;
using System.Configuration;

namespace SmsSender.Services.Concrete
{
    public class TFonTelekomManager : ISmsService
    {
        public string Sender(string phone, string message)
        {
            phone = PhoneHelper.PhoneCustomizer(phone);
            string istekAdresi = "http://api2.ekomesaj.com/json/syncreply/SendInstantSms";
            var Credential = new
            {
                Username = ConfigurationManager.AppSettings["username"],
                Password = ConfigurationManager.AppSettings["password"],
                ResellerID = 1111
            };
            var Sms = new
            {
                SmsCoding = "String",
                SenderName = ConfigurationManager.AppSettings["orginator"],
                Route = 0,
                ValidityPeriod = 0,
                DataCoding = "Default",
                ToMsisdns = new
                {
                    Msisdn = phone,
                    Name = "",
                    Surname = "",
                    CustomField1 = "",
                },
                ToGroups = new List<int>(),
                IsCreateFromTeplate = false,
                SmsTitle = ConfigurationManager.AppSettings["orginator"],
                SmsContent = message,
                RequestGuid = "",
                CanSendSmsToDuplicateMsisdn = false,
                SmsSendingType = "ByNumber"
            };
            return Statik.CreateWebRequest(istekAdresi, new { Credential, Sms }.ToJsonSerialize(), "POST", "application/json");
        }
    }
}
