using Pluralsight.AWS.Utils;
using System;


namespace Pluralsight.AWS.IAM.Native.WinUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var newUserName = Guid.NewGuid().ToString();
            var timestamp = Helper.CalculateTimestamp();

            Console.WriteLine("** Pluralsight Course Demo - Native HTTP Calls **");
            Console.WriteLine("** Pluralsight Course Demo - Create IAM User **");

            //create string to sign 
            var stringToConvert = "GET\n" +
                                  "iam.amazonaws.com\n" +
                                  "/\n" +
                                  // must be alpha ordered
                                  $"AWSAccessKeyId={Confidential.AwsAccessKeyId}" +
                                  "&Action=CreateUser" +
                                  "&Path=%2FIT%2Farchitecture%2F" + //encode path directly
                                  "&SignatureMethod=HmacSHA1" +
                                  "&SignatureVersion=2" +
                                  $"&Timestamp={timestamp}" +
                                  $"&UserName={newUserName}" +
                                  "&Version=2010-05-08";

            var urlEncodedCanonical = Helper.CreateUrlEncodedSignature(stringToConvert);

            //actual URL string
            var iamUrl = "https://iam.amazonaws.com/?Action=CreateUser" +
                         "&Path=%2FIT%2Farchitecture%2F" + //encode path directly
                         $"&UserName={newUserName}" +
                         "&Version=2010-05-08" +
                         $"&Timestamp={timestamp}" +
                         $"&Signature={urlEncodedCanonical}" +
                         "&SignatureVersion=2" +
                         "&SignatureMethod=HmacSHA1" +
                         $"&AWSAccessKeyId={Confidential.AwsAccessKeyId}";

            var xdoc = Helper.MakeWebRequest(iamUrl);

            Console.WriteLine("User created.");

            Console.WriteLine(xdoc.ToString());

            Console.ReadLine();
        }
    }
}
