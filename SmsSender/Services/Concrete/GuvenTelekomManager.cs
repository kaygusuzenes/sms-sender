using SmsSender.Services.Abstract;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Configuration;
using SmsSender.Helpers;

namespace SmsSender.Services.Concrete
{
    public class GuvenTelekomManager : ISmsService
    {
        public string Sender(string phone, string message)
        {
            phone = PhoneHelper.PhoneCustomizer(phone);
            string returnValue = string.Empty;
            string IstekAdresi = "http://api.guventelekom.net:8080/api/smspost/v1";
            HttpWebRequest request = WebRequest.Create(new Uri(IstekAdresi)) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = 5000;
            byte[] data = UTF8Encoding.UTF8.GetBytes(CreateXmlGuvenTelekom(phone, message)); request.ContentLength = data.Length;
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

        private string CreateXmlGuvenTelekom(string phone, string message)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.Unicode;
            settings.Indent = true;
            settings.IndentChars = ("	");
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartElement("sms");
                writer.WriteElementString("username", ConfigurationManager.AppSettings["username"]);
                writer.WriteElementString("password", ConfigurationManager.AppSettings["password"]);
                writer.WriteElementString("header", ConfigurationManager.AppSettings["orginator"]);
                writer.WriteElementString("validity", "2880");
                writer.WriteStartElement("message");
                writer.WriteStartElement("gsm");
                writer.WriteElementString("no", phone);
                writer.WriteEndElement(); //gsm
                writer.WriteStartElement("msg");
                writer.WriteCData(Statik.ReplaceTRChar(message));
                writer.WriteEndElement(); //msg 
                writer.WriteEndElement(); //message 
                writer.WriteEndElement(); // sms 
                writer.Flush();
            }
            return sb.ToString();
        }
    }
}
