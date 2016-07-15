using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace Pluralsight.AWS.Utils
{
    public static class Helper
    {
        public static string CalculateTimestamp()
        {
            var now = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            var timestamp = HttpUtility.UrlEncode(now)?.Replace("%3a", "%3A");
            return timestamp;
        }

        public static string CreateUrlEncodedSignature(string stringToConvert)
        {
            var encodedCanonical = CreateSignature(stringToConvert);

            var urlEncodedCanonical = HttpUtility.UrlEncode(encodedCanonical);

            return urlEncodedCanonical;
        }

        public static string CreateSignature(string stringToConvert)
        {
            Encoding ae = new UTF8Encoding();

            var signature = new HMACSHA1
            {
                Key = ae.GetBytes(Confidential.AwsSecretAccessKey)
            };

            var bytes = ae.GetBytes(stringToConvert);

            var moreBytes = signature.ComputeHash(bytes);

            var encodedCanonical = Convert.ToBase64String(moreBytes);

            return encodedCanonical;
        }

        public static XDocument MakeWebRequest(string url)
        {
            var req = WebRequest.Create(url) as HttpWebRequest;

            return MakeWebRequest(req);
        }

        public static XDocument MakeWebRequest(HttpWebRequest req)
        {
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                var reader = new StreamReader(resp.GetResponseStream());

                var responseXml = reader.ReadToEnd();

                var doc = XDocument.Parse(responseXml);

                return doc;
            }
        }
    }
}
