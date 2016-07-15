using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Pluralsight.AWS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pluralsight.AWS.EBS.NetSdk.WinUI
{
    static class Program
    {

        public static void Main(string[] args)
        {
            Console.WriteLine("** Pluralsight Course Demo - SDK Calls **");

            var client = new AmazonEC2Client(RegionEndpoint.EUWest1);

            var availabilityZone = client.GetEc2InstanceAvailabilityZone();

            var snapShotId = client.CreateSnapshot();
            Console.WriteLine($"Snapshot {snapShotId} created");
            Console.ReadLine();

            var volumeId = client.CreateVolumeFromSnapshot(availabilityZone, snapShotId);
            Console.WriteLine($"Volume {volumeId} created");
            Console.ReadLine();

            client.AttachVolume(volumeId);
            Console.WriteLine($"Volume {volumeId} attached to instance.");
            Console.ReadLine();
            
            client.DetachVolume(volumeId);
            Console.WriteLine($"Volume {volumeId} detached from instance");
            Console.ReadLine();
            
            client.DeleteVolume(volumeId);
            Console.WriteLine($"Volume {volumeId} deleted");
            Console.ReadLine();

            client.DeleteSnapshot(snapShotId);
            Console.WriteLine($"Snapshot {snapShotId} deleted");
            Console.ReadLine();
        }

        private static string GetEc2InstanceAvailabilityZone(this IAmazonEC2 client)
        {
            var request = new DescribeInstancesRequest
            {
                InstanceIds = new List<string> {Confidential.Ec2TestInstance}
            };

            
            var response = client.DescribeInstances(request);

            var availabilityZone = response
                .Reservations.FirstOrDefault()
                ?.Instances.FirstOrDefault()
                ?.Placement?.AvailabilityZone;

            return availabilityZone;
        }

        private static string CreateSnapshot(this IAmazonEC2 client)
        {
            var request = new CreateSnapshotRequest
            {
                Description = $"Description-{Guid.NewGuid()}",
                VolumeId = Confidential.VolumeId
            };

            var response = client.CreateSnapshot(request);

            return response.Snapshot.SnapshotId;
        }

        private static string CreateVolumeFromSnapshot(this IAmazonEC2 client, string availabilityZone, string snapShotId)
        {
            var request = new CreateVolumeRequest(availabilityZone, snapShotId)
            {
                Size = 5
            };

            var response = client.CreateVolumeAsync(request).Result;

            return response.Volume.VolumeId;
        }

        private static void AttachVolume(this IAmazonEC2 client, string volumeId)
        {
            var request = new AttachVolumeRequest
            {
                Device = "xvdl",
                InstanceId = Confidential.Ec2TestInstance,
                VolumeId = volumeId
            };

            var response = client.AttachVolume(request);
        }

        private static void DetachVolume(this IAmazonEC2 client, string volumeId)
        {

            var request = new DetachVolumeRequest
            {
                VolumeId = volumeId,
                InstanceId = Confidential.Ec2TestInstance,
                Device = "xvdl",
                Force = true
            };


            var response = client.DetachVolume(request);
        }

        private static void DeleteVolume(this IAmazonEC2 client, string volumeId)
        {

            var request = new DeleteVolumeRequest
            {
                VolumeId = volumeId
            };

            var response = client.DeleteVolume(request);
        }

        private static void DeleteSnapshot(this IAmazonEC2 client, string snapShotId)
        {

            var request = new DeleteSnapshotRequest
            {
                SnapshotId = snapShotId
            };

            var response = client.DeleteSnapshot(request);
        }


    }
}