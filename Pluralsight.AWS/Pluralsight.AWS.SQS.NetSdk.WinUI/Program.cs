using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pluralsight.AWS.SQS.NetSdk.WinUI
{
    static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("** Pluralsight Course Demo - SDK Calls **");

            var client = new AmazonSQSClient(RegionEndpoint.EUWest1);

            var queues = client.ListQueues();

            var queueUrl = queues.First();

            client.ShowQueueAttributes(queueUrl);

            client.SendMessages(queueUrl, 10);

            client.ReadAndDelete(queueUrl);
        }

        private static IEnumerable<string> ListQueues(this IAmazonSQS client)
        {
            Console.WriteLine("** Pluralsight Course Demo - List Queues **");

            var request = new ListQueuesRequest();
            
            var response = client.ListQueues(request);

            foreach (var queue in response.QueueUrls)
            {
                Console.WriteLine($"Url is {queue}");
            }

            return response.QueueUrls;
        }

        private static void ShowQueueAttributes(this IAmazonSQS client, string url)
        {
            Console.WriteLine("** Pluralsight Course Demo - Get Queue Attributes **");

            var request = new GetQueueAttributesRequest()
            {
                QueueUrl = url,
                AttributeNames = new List<string>
                {
                    "All"
                }
            };

            var response = client.GetQueueAttributes(request);

            foreach (var pair in response.Attributes)
            {
                Console.WriteLine($"{pair.Key} : {pair.Value}");
            }
        }

        private static void SendMessages(this IAmazonSQS client, string url, int count)
        {
            Console.WriteLine("** Pluralsight Course Demo - Send to Queue **");

            for (int i = 0; i < count; i++)
            {
                var response = client.SendMessage(new SendMessageRequest
                {
                    DelaySeconds = 0,
                    QueueUrl = url,
                    MessageBody = $"this is a message {i} from the SDK"
                });
                Console.WriteLine($"Message {response.MessageId} sent");
            }

            Console.ReadLine();
        }

        private static void ReadAndDelete(this IAmazonSQS client, string url)
        {
            Console.WriteLine("** Pluralsight Course Demo - Receive/Delete From Queue **");

            var request = new ReceiveMessageRequest
            {
                AttributeNames = new List<string>() {"All"},
                MaxNumberOfMessages = 10,
                QueueUrl = url,
            };

            var queueMessages = new List<Message>();

            var messagesFound = true;

            while (messagesFound)
            {
                var response = client.ReceiveMessage(request);

                messagesFound = response.Messages.Any();

                if (!messagesFound) continue;

                queueMessages.AddRange(response.Messages);

                Console.WriteLine("Queue messages received; count is " + response.Messages.Count);
            }

            Console.ReadLine();

            while (queueMessages.Any())
            {
                var batchRequest = new DeleteMessageBatchRequest
                {
                    QueueUrl = url,
                    Entries = new List<DeleteMessageBatchRequestEntry>()
                };


                var deletions = queueMessages
                    .Take(10)
                    .Select(msg => new DeleteMessageBatchRequestEntry
                    {
                        ReceiptHandle = msg.ReceiptHandle,
                        Id = msg.MessageId
                    });

                foreach (var item in deletions)
                {
                    batchRequest.Entries.Add(item);
                    Console.WriteLine($"Message {item.Id} deleted");
                }

                var batchResponse = client.DeleteMessageBatch(batchRequest);

                queueMessages.RemoveRange(0, 10);
            }

            Console.ReadLine();


        }
    }
}