using SmsSender.Helpers;
using SmsSender.Services.Abstract;
using System;
using System.Collections.Generic;

namespace SmsSender.Services.Concrete
{
    internal class RelatedDigitalManager : ISmsService
    {
        public string Sender(string phone, string message)
        {
            phone = PhoneHelper.PhoneCustomizer(phone);
            var apiAyar = UIAyarlar.SiteAyarlari.DijitalPazarlama.RelatedDigital.ApiAyar;
            var client = new RelatedDigital.RelatedDigitalClient(apiAyar.KullaniciAdi, apiAyar.Sifre);
            var smsRequest = new RelatedDigital.PostSMSRequest();
            smsRequest.Originator = "";
            smsRequest.NumberMessagePair = new List<RelatedDigital.NumberMessagePair>
            {
                new RelatedDigital.NumberMessagePair{ Key = phone, Value = message},
            };
            smsRequest.BeginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            smsRequest.EndTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");

            var response = client.SmsGonder(smsRequest);
            return response.DetailedMessage;
        }
    }
}
