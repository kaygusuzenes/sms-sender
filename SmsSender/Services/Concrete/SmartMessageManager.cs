using SmsSender.Helpers;
using SmsSender.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace SmsSender.Services.Concrete
{
    public class SmartMessageManager : ISmsService
    {
        public string Sender(string phone, string message)
        {
            phone = PhoneHelper.PhoneCustomizer(phone);
            string returnValue = string.Empty;
            string IstekAdresi = "http://api2.smartmessage-engage.com/GET/SMS";
            List<string> Params = new List<string>();
            Params.Add("UserName=" + ConfigurationManager.AppSettings["username"]);
            Params.Add("Password=" + ConfigurationManager.AppSettings["password"]);
            Params.Add("JobId=" + ConfigurationManager.AppSettings["orginator"].Split('|')[1]);
            Params.Add("Message=" + message);
            Params.Add("MobilePhone=" + phone);
            Params.Add("CustomerNo=" + ConfigurationManager.AppSettings["orginator"].Split('|')[0]);
            Params.Add("PlannedSendingDate=" + DateTime.Now.AddMinutes(1));
            string postData = String.Join("&", Params.ToArray());
            returnValue = Statik.CreateWebRequest(IstekAdresi, postData, "GET", "", new List<KeyValuePair<string, string>>());
            return returnValue;
        }
    }
}
