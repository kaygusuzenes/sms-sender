using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmsSender
{
    internal class old
    {
        public static string SmsGonder(string Telefon, string Mesaj)
        {
            string returnValue = string.Empty;

            if (!string.IsNullOrEmpty(Telefon))
            {
                Telefon = Telefon.Replace("+", string.Empty).Replace(" ", string.Empty);
                if (Telefon.Length == 10)
                {
                    Telefon = "90" + Telefon;
                }
                else if (Telefon.Length == 11)
                {
                    Telefon = "9" + Telefon;
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                if (ayarlar.ServisSaglayici == (int)Enums.SmsServisSaglayici.GuvenTelekom)
                {
                    returnValue = SmsGonderGuvenTelekom(Telefon, Mesaj);
                }
                else if (ayarlar.ServisSaglayici == (int)Enums.SmsServisSaglayici.TFonTelekom)
                {
                    returnValue = SmsGonderTFonTelekom(Telefon, Mesaj);
                }
                else if (ayarlar.ServisSaglayici == (int)Enums.SmsServisSaglayici.NetGsm)
                {
                    returnValue = SmsGonderNetGsm(Telefon, Mesaj);
                }
                else if (ayarlar.ServisSaglayici == (int)Enums.SmsServisSaglayici.SmartMessage)
                {
                    returnValue = SmsGonderSmartMessage(Telefon, Mesaj);
                }
                else if (ayarlar.ServisSaglayici == (int)Enums.SmsServisSaglayici.RelatedDigital)
                {
                    returnValue = SmsGonderRelatedDigital(Telefon, Mesaj);
                }
                else if (ayarlar.ServisSaglayici == (int)Enums.SmsServisSaglayici.Telsam)
                {
                    returnValue = SmsGonderTelsam(Telefon, Mesaj);
                }
                else if (ayarlar.ServisSaglayici == (int)Enums.SmsServisSaglayici.Verimor)
                {
                    returnValue = SmsGonderVerimor(Telefon, Mesaj);
                }
            }
            return returnValue;
        }

        #region guven
        private static string SmsGonderGuvenTelekom(string tel, string mesaj)
        {
            string returnValue = string.Empty;
            string IstekAdresi = "http://api.guventelekom.net:8080/api/smspost/v1";

            HttpWebRequest request = WebRequest.Create(new Uri(IstekAdresi)) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = 5000;
            byte[] data = UTF8Encoding.UTF8.GetBytes(createXmlGuvenTelekom(tel, mesaj)); request.ContentLength = data.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(data, 0, data.Length);
            }
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                returnValue = reader.ReadToEnd();
            }
            return returnValue;
        }

        private static string createXmlGuvenTelekom(string tel, string mesaj)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.Unicode;
            settings.Indent = true;
            settings.IndentChars = ("	");
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartElement("sms");
                writer.WriteElementString("username", ayarlar.Kullanici);
                writer.WriteElementString("password", ayarlar.Sifre);
                writer.WriteElementString("header", ayarlar.Orginator);
                writer.WriteElementString("validity", "2880");
                writer.WriteStartElement("message");
                writer.WriteStartElement("gsm");
                writer.WriteElementString("no", tel);
                writer.WriteEndElement(); //gsm
                writer.WriteStartElement("msg");
                writer.WriteCData(Statik.ReplaceTRChar(mesaj));
                writer.WriteEndElement(); //msg 
                writer.WriteEndElement(); //message 
                writer.WriteEndElement(); // sms 
                writer.Flush();
            }
            return sb.ToString();
        }

        #endregion

        #region tfon
        private static string SmsGonderTFonTelekom(string tel, string mesaj)
        //{
        //    string istekAdresi = "http://api2.ekomesaj.com/json/syncreply/SendInstantSms";

        //    var Credential = new
        //    {
        //        Username = ayarlar.Kullanici,
        //        Password = ayarlar.Sifre,
        //        ResellerID = 1111
        //    };
        //    var Sms = new
        //    {
        //        SmsCoding = "String",
        //        SenderName = ayarlar.Orginator,
        //        Route = 0,
        //        ValidityPeriod = 0,
        //        DataCoding = "Default",
        //        ToMsisdns = new
        //        {
        //            Msisdn = tel,
        //            Name = "",
        //            Surname = "",
        //            CustomField1 = "",
        //        },
        //        ToGroups = new List<int>(),
        //        IsCreateFromTeplate = false,
        //        SmsTitle = ayarlar.Orginator,
        //        SmsContent = mesaj,
        //        RequestGuid = "",
        //        CanSendSmsToDuplicateMsisdn = false,
        //        SmsSendingType = "ByNumber"
        //    };
        //    return Statik.CreateWebRequest(istekAdresi, new { Credential, Sms }.ToJsonSerialize(), "POST", "application/json");
        //}
        #endregion

        #region netgsm
        private static string SmsGonderNetGsm(string tel, string mesaj)
        {
            string returnValue = string.Empty;
            string IstekAdresi = "https://api.netgsm.com.tr/xmlbulkhttppost.asp";
            string requestXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                            <mainbody>
                                <header>
                                    <company dil=""TR"" bayikodu=""11111"">Ticimax</company>
                                    <usercode>" + ayarlar.Kullanici + @"</usercode>
                                    <password>" + ayarlar.Sifre + @"</password>
                                    <startdate></startdate>
                                    <stopdate></stopdate>
                                    <type>1:n</type>
                                    <msgheader>" + ayarlar.Orginator + @"</msgheader>
                                </header>
                                <body>
                                    <msg><![CDATA[" + mesaj + @"]]></msg>
                                    <no>" + tel + @"</no>
                                </body>
                            </mainbody>";
            returnValue = Statik.CreateWebRequest(IstekAdresi, requestXml, "POST", "application/x-www-form-urlencoded", new List<KeyValuePair<string, string>>());
            return returnValue;
        }
        #endregion

        #region jetmesaj
        private static string SmsGonderJetMesaj(string tel, string mesaj)
        {
            string returnValue = "";
            string soapStr = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                                  <soap:Body>
                                    <SmsGonder xmlns=""http://tempuri.org/"">
                                      <kullaniciAd>" + ayarlar.Kullanici + @"</kullaniciAd>
                                      <parola>" + ayarlar.Sifre + @"</parola>
                                      <gsmNo>
                                        <string>" + tel + @"</string>
                                      </gsmNo>
                                      <smsText>
                                        <string>" + mesaj + @"</string>
                                      </smsText>
                                      <gonderimTarihi>" + DateTime.Now.ToString("ddMMyyyyHHmmss") + @"</gonderimTarihi>
                                      <alfaNumeric>" + ayarlar.Orginator + @"</alfaNumeric>
                                      <chargedNumber></chargedNumber>
                                      <multiSms>false</multiSms>
                                    </SmsGonder>
                                  </soap:Body>
                                </soap:Envelope>";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://92.42.35.50:16899/smswebservice.asmx");
            req.Headers.Add("SOAPAction", "\"http://tempuri.org/SmsGonder\"");
            req.ContentType = "text/xml;charset=\"utf-8\"";
            req.Accept = "text/xml";
            req.Method = "POST";

            using (Stream stm = req.GetRequestStream())
            {
                ;
                using (StreamWriter stmw = new StreamWriter(stm))
                {
                    stmw.Write(soapStr);
                }
            }

            using (StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                string result = responseReader.ReadToEnd();
                XDocument ResultXML = XDocument.Parse(result);
                returnValue = ResultXML.Descendants(XName.Get("SmsGonderResponse", "http://tempuri.org/")).FirstOrDefault().Element(XName.Get("SmsGonderResult", "http://tempuri.org/")).Value.Split(':')[0];
            }
            return returnValue;
        }
        #endregion

        #region smartmessage
        private static string SmsGonderSmartMessage(string tel, string mesaj)
        {
            string returnValue = string.Empty;
            string IstekAdresi = "http://api2.smartmessage-engage.com/GET/SMS";
            List<string> Params = new List<string>();
            Params.Add("UserName=" + ayarlar.Kullanici);
            Params.Add("Password=" + ayarlar.Sifre);
            Params.Add("JobId=" + ayarlar.Orginator.Split('|')[1]);
            Params.Add("Message=" + mesaj);
            Params.Add("MobilePhone=" + tel);
            Params.Add("CustomerNo=" + ayarlar.Orginator.Split('|')[0]);
            Params.Add("PlannedSendingDate=" + DateTime.Now.AddMinutes(1));
            string postData = String.Join("&", Params.ToArray());
            returnValue = Statik.CreateWebRequest(IstekAdresi, postData, "GET", "", new List<KeyValuePair<string, string>>());
            return returnValue;
        }

        #endregion

        #region relateddigital
        private static string SmsGonderRelatedDigital(string tel, string mesaj)
        {
            var apiAyar = UIAyarlar.SiteAyarlari.DijitalPazarlama.RelatedDigital.ApiAyar;
            var client = new RelatedDigital.RelatedDigitalClient(apiAyar.KullaniciAdi, apiAyar.Sifre);
            var smsRequest = new RelatedDigital.PostSMSRequest();
            smsRequest.Originator = "";
            smsRequest.NumberMessagePair = new List<RelatedDigital.NumberMessagePair>
            {
                new RelatedDigital.NumberMessagePair{ Key = tel, Value = mesaj},
            };
            smsRequest.BeginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            smsRequest.EndTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");

            var response = client.SmsGonder(smsRequest);
            return response.DetailedMessage;
        }

        #endregion

        #region telsam

        private static string SmsGonderTelsam(string tel, string mesaj)
        {
            string returnValue = string.Empty;
            string IstekAdresi = "http://websms.telsam.com.tr/xmlapi/sendsms";
            tel = TelsamTelFix(tel);
            string requestXml = @"<?xml version=""1.0""?>
                                <SMS>
                                  <authentication>
                                    <username>" + ayarlar.Kullanici + @"</username>
                                    <password>" + ayarlar.Sifre + @"</password>
                                  </authentication>
                                  <message>
                                    <originator>" + ayarlar.Orginator + @"</originator>
                                    <text>" + mesaj + @"</text>
                                    <unicode></unicode>
                                    <international></international>
                                    <canceltext></canceltext>
                                  </message>
                                  <receivers>
                                    <receiver>" + tel + @"</receiver>
                                  </receivers>
                                </SMS>";
            returnValue = Statik.CreateWebRequest(IstekAdresi, requestXml, "POST", "application/x-www-form-urlencoded", new List<KeyValuePair<string, string>>());
            return returnValue;
        }
        private static string TelsamTelFix(string telefon)
        {
            if (telefon.Length == 12)
            {
                telefon = telefon.Substring(2, 10);
            }
            else if (telefon.Length == 11)
            {
                telefon = telefon.Substring(1, 10);
            }
            return telefon;
        }
        #endregion

        #region verimor
        private static string SmsGonderVerimor(string tel, string mesaj)
        {
            string istekAdresi = "http://sms.verimor.com.tr/v2/send.json";
            var Sms = new
            {
                username = ayarlar.Kullanici,
                password = ayarlar.Sifre,
                source_addr = ayarlar.Orginator,
                valid_for = "24:00",
                datacoding = "1",
                messages = new List<object>
                {
                    new
                    {
                       msg = mesaj,
                       dest = tel
                    }
                }
            };
            return Statik.CreateWebRequest(istekAdresi, Sms.ToJsonSerialize(), "POST", "application/json");
        }
        #endregion




    }
}