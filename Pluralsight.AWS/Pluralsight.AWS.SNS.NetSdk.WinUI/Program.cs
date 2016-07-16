using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Pluralsight.AWS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Pluralsight.AWS.SNS.NetSdk.WinUI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("** Pluralsight Course Demo - SDK Calls **");

            var client = new AmazonSimpleNotificationServiceClient(RegionEndpoint.EUWest1);

            var topicArn = client.CreateNewTopic();

            var allTopics = client.GetAllTopics();

            client.ShowTopicAttributes(allTopics);
            
            client.SubscribeToTopic(topicArn);

            client.ListTopicSubscriptions(topicArn);

            client.SendMessageToTopic(topicArn);

            Console.Read();

            client.DeleteTopic(topicArn);
        }

        private static string CreateNewTopic(this IAmazonSimpleNotificationService client)
        {
            var response = client.CreateTopic($"topic-{Guid.NewGuid()}");

            Thread.Sleep(2000);

            return response.TopicArn;
        }

        private static IReadOnlyList<string> GetAllTopics(this IAmazonSimpleNotificationService client)
        {
            Console.WriteLine("** Pluralsight Course Demo - List SNS Topics **");

            var request = new ListTopicsRequest();

            var topics = client.ListTopics(request);

            return topics.Topics.Select(topic => topic.TopicArn).ToArray();
        }

        private static void ShowTopicAttributes(this IAmazonSimpleNotificationService client, IEnumerable<string> arns)
        {
            Console.WriteLine("** Pluralsight Course Demo - Show SNS Topic Attributes **");

            foreach (var arn in arns)
            {
                var response = client.GetTopicAttributes(arn);

                Console.WriteLine($"Topic {arn}");

                foreach (var attribute in response.Attributes)
                {
                    Console.WriteLine($"  {attribute.Key} = {attribute.Value}");   
                }
            }
        }

        private static void SubscribeToTopic(this IAmazonSimpleNotificationService client, string topicArn)
        {
            Console.WriteLine("** Pluralsight Course Demo - Subscribe to Topic via SMS **");

            var request = new SubscribeRequest
            {
                Protocol = "sms",
                Endpoint = Confidential.PhoneNumber,
                TopicArn = topicArn
            };

            var response = client.Subscribe(request);

            Console.WriteLine($"Subscription {response.SubscriptionArn} to {topicArn} created.");
        }

        private static void ListTopicSubscriptions(this IAmazonSimpleNotificationService client, string topicArn)
        {
            Console.WriteLine("** Pluralsight Course Demo - List SNS Topic Subscriptions **");
            
            var request = new ListSubscriptionsByTopicRequest(topicArn);

            var response = client.ListSubscriptionsByTopic(request);

            foreach (var s in response.Subscriptions)
            {
                Console.WriteLine("Subscription: " + s.Endpoint);
            }
        }
        private static void SendMessageToTopic(this IAmazonSimpleNotificationService client, string topicArn)
        {
            Console.WriteLine("** Pluralsight Course Demo - Subscribe to Topic **");

            var request = new PublishRequest
            {
                TopicArn = topicArn,
                Subject = $"Topic {topicArn}",
                Message = $"This is a test message for topic {topicArn}"
            };

            var response = client.Publish(request);

            Console.WriteLine($"Message sent to topic {topicArn}.");
        }
    }
}