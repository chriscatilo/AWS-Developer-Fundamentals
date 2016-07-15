using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Pluralsight.AWS.Utils;
using System;
using System.Collections.Generic;

namespace Pluralsight.AWS.EC2.NetSdk.WinUI
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("** Pluralsight Course Demo - SDK Calls **");

            var instanceIds = new List<string>() { Confidential.Ec2TestInstance };

            AddTag(instanceIds);

            StopInstances(instanceIds);

        }

        private static void AddTag(List<string> instanceIds)
        {
            Console.WriteLine("** Pluralsight Course Demo - Add EC2 Tag **");

            var tags = new List<Tag>
            {
                new Tag {Key = $"Key-{Guid.NewGuid()}", Value = $"Value-{Guid.NewGuid()}"}
            };

            var request = new CreateTagsRequest
            {
                Resources = instanceIds,
                Tags = tags
            };

            
            var client = new AmazonEC2Client(RegionEndpoint.EUWest1);

            var response = client.CreateTags(request);

            Console.WriteLine("Tag created on EC2 instance");

            Console.ReadLine();
        }

        private static void StopInstances(List<string> instanceIds)
        {
            Console.WriteLine("** Pluralsight Course Demo - Terminate Instance **");

            var client = new AmazonEC2Client(RegionEndpoint.EUWest1);

            var tRequest = new StopInstancesRequest
            {
                InstanceIds = instanceIds
            };

            var response = client.StopInstances(tRequest);

            Console.WriteLine("EC2 instance stopped");

            Console.ReadLine();
        }
    }
}