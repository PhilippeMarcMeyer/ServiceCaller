using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
namespace ServiceCaller
{
    class Program
    {
            public static void Execute()
            {

            HttpWebRequest request = CreateWebRequest();
                XmlDocument soapEnvelopeXml = new XmlDocument();
                soapEnvelopeXml.LoadXml(string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                  <soap:Body>
                    <Add xmlns=""http://tempuri.org/"">
                      <intA>{0}</intA>
                      <intB>{1}</intB>
                    </Add>
                  </soap:Body>
                </soap:Envelope>",5,7));

                using (Stream stream = request.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        string soapResult = rd.ReadToEnd();

                    XDocument doc = XDocument.Parse(soapResult);
                    XNamespace ns = "http://tempuri.org/";
                    XElement addResponse = doc.Descendants(ns + "AddResponse").ToList().FirstOrDefault();
                    int  responseValue = (int)addResponse.Element(ns + "AddResult");
                    Console.WriteLine(responseValue);
                        Console.ReadLine();
                    }
                }
            }
            /// <summary>
            /// Create a soap webrequest to [Url]
            /// </summary>
            /// <returns></returns>
            public static HttpWebRequest CreateWebRequest()
            {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"http://www.dneonline.com/calculator.asmx?op=Add");
                webRequest.Headers.Add(@"SOAP:Action");
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";
                return webRequest;
            }

            static void Main(string[] args)
            {
                Execute();
            }
    }
}
