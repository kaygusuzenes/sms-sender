using SmsSender.Helpers;
using SmsSender.Services.Abstract;
using System.Collections.Generic;
using System.Configuration;

namespace SmsSender.Services.Concrete
{
    public class TelsamManager : ISmsService
    {
        public string Sender(string phone, string message)
        {
            phone = PhoneHelper.PhoneCustomizer(phone);
            string returnValue = string.Empty;
            string IstekAdresi = "http://websms.telsam.com.tr/xmlapi/sendsms";
            phone = TelsamPhoneFix(phone);
            string requestXml = @"<?xml version=""1.0""?>
                                <SMS>
                                  <authentication>
                                    <username>" + ConfigurationManager.AppSettings["username"] + @"</username>
                                    <password>" + ConfigurationManager.AppSettings["password"] + @"</password>
                                  </authentication>
                                  <message>
                                    <originator>" + ConfigurationManager.AppSettings["orginator"] + @"</originator>
                                    <text>" + message + @"</text>
                                    <unicode></unicode>
                                    <international></international>
                                    <canceltext></canceltext>
                                  </message>
                                  <receivers>
                                    <receiver>" + phone + @"</receiver>
                                  </receivers>
                                </SMS>";
            returnValue = Statik.CreateWebRequest(IstekAdresi, requestXml, "POST", "application/x-www-form-urlencoded", new List<KeyValuePair<string, string>>());
            return returnValue;
        }

        private string TelsamPhoneFix(string phone)
        {
            if (phone.Length == 12)
            {
                phone = phone.Substring(2, 10);
            }
            else if (phone.Length == 11)
            {
                phone = phone.Substring(1, 10);
            }
            return phone;
        }
    }
}
