using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using System;
using System.Threading;

namespace Pluralsight.AWS.IAM.NetSdk.WinUI
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("** Pluralsight Course Demo - SDK Calls **");

            var userName = CreateUser();

            AddPolicy(userName);

            DeletePolicy(userName);

            DeleteUser(userName);
        }

        private static string CreateUser()
        {
            Console.WriteLine("** Pluralsight Course Demo - Create IAM User **");

            var client = new AmazonIdentityManagementServiceClient();
            var userName = $"sdk-{Guid.NewGuid()}";

            var request = new CreateUserRequest
            {
                UserName = userName,
                Path = "/IT/architecture/"
            };

            var response = client.CreateUser(request);

            Console.WriteLine($"User {userName} created.");

            return userName;
        }

        /// <summary>
        /// Add a user policy that controls the user's access to the AWS products and resources
        /// </summary>
        private static void AddPolicy(string userName)
        {
            Console.WriteLine("** Pluralsight Course Demo - Add IAM Policy **");

            // generated from https://awspolicygen.s3.amazonaws.com/policygen.html
            const string policyDocument = "{\"Statement\": [{\"Sid\": \"Stmt1327985669302\", \"Action\": [ \"ec2:DescribeInstances\"], \"Effect\": \"Allow\", \"Resource\": \"*\" }]}";


            var policyRequest = new PutUserPolicyRequest
            {
                UserName = userName,
                PolicyName = "EC2Policy",
                PolicyDocument = policyDocument
            };


            var client = new AmazonIdentityManagementServiceClient();

            var policyResponse = client.PutUserPolicy(policyRequest);

            Console.WriteLine("Policy added.");

            Thread.Sleep(2000);
        }

        private static void DeletePolicy(string userName)
        {
            Console.WriteLine("** Pluralsight Course Demo - Add IAM Policy **");

           
            var policyRequest = new DeleteUserPolicyRequest
            {
                UserName = userName,
                PolicyName = "EC2Policy"
            };


            var client = new AmazonIdentityManagementServiceClient();

            var policyResponse = client.DeleteUserPolicy(policyRequest);

            Console.WriteLine("Policy added.");

            Thread.Sleep(2000);
        }

        private static void DeleteUser(string userName)
        {
            Console.WriteLine("** Pluralsight Course Demo - Delete IAM User **");

            var client = new AmazonIdentityManagementServiceClient();

            var request = new DeleteUserRequest
            {
                UserName = userName
            };

            var response = client.DeleteUser(request);

            Console.WriteLine($"User {userName} deleted.");

            Thread.Sleep(2000);
        }
    }
}