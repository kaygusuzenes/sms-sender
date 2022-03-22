using SmsSender.Helpers;
using SmsSender.Services.Abstract;
using System.Collections.Generic;
using System.Configuration;

namespace SmsSender.Services.Concrete
{
    internal class NetGsmManager : ISmsService
    {
        public string Sender(string phone, string message)
        {
            phone = PhoneHelper.PhoneCustomizer(phone);
            string returnValue = string.Empty;
            string IstekAdresi = "https://api.netgsm.com.tr/xmlbulkhttppost.asp";
            string requestXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                            <mainbody>
                                <header>
                                    <company dil=""TR"" bayikodu=""11111"">Ticimax</company>
                                    <usercode>" + ConfigurationManager.AppSettings["username"] + @"</usercode>
                                    <password>" + ConfigurationManager.AppSettings["password"] + @"</password>
                                    <startdate></startdate>
                                    <stopdate></stopdate>
                                    <type>1:n</type>
                                    <msgheader>" + ConfigurationManager.AppSettings["orginator"] + @"</msgheader>
                                </header>
                                <body>
                                    <msg><![CDATA[" + message + @"]]></msg>
                                    <no>" + phone + @"</no>
                                </body>
                            </mainbody>";
            returnValue = Statik.CreateWebRequest(IstekAdresi, requestXml, "POST", "application/x-www-form-urlencoded", new List<KeyValuePair<string, string>>());
            return returnValue;

        }
    }
}
