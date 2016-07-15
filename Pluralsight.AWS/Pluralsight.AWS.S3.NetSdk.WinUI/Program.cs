using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading;

namespace Pluralsight.AWS.S3.NetSdk.WinUI
{
    static class Program
    {
        const string BucketName = "kodigouk";
        const string FileS3Key = "watson10.jpg";

        public static void Main(string[] args)
        {
            Console.WriteLine("** Pluralsight Course Demo - SDK Calls **");

            var client = new AmazonS3Client(RegionEndpoint.EUWest1);

            client.ListMyBuckets();
            client.AddToMyBucket();
            client.DeleteObjectFromBucket();
            Console.ReadLine();
        }

        private static void ListMyBuckets(this IAmazonS3 client)
        {
            Console.WriteLine("** Pluralsight Course Demo - List buckets **");

            var response = client.ListBuckets();
            foreach (var bucket in response.Buckets)
            {
                Console.WriteLine("Bucket: " + bucket.BucketName);
            }

            Thread.Sleep(2000);
        }

        private static void AddToMyBucket(this IAmazonS3 client)
        {
            Console.WriteLine("** Pluralsight Course Demo - Add object to bucket **");

            
            var request = new PutObjectRequest
            {   
                BucketName = BucketName,
                FilePath = @"watson.jpg",
                Key = FileS3Key,
                ContentType = "image/jpeg"
            };

            var response = client.PutObject(request);

            Console.WriteLine("S3 object added.");

            Thread.Sleep(2000);
        }

        private static void DeleteObjectFromBucket(this IAmazonS3 client)
        {
            Console.WriteLine("** Pluralsight Course Demo - Delete object from bucket **");

            var request = new DeleteObjectRequest()
            {
                BucketName = BucketName,
                Key = FileS3Key
            };

            var response = client.DeleteObject(request);
            Console.WriteLine("S3 object deleted.");

            Thread.Sleep(2000);
        }
    }
}