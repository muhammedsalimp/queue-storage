using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using System;

namespace MessageSender.Services
{
    public class QueueService : IQueueService
    {
        private readonly IConfiguration _configuration;

        public QueueService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void SendMessage(string queueName, string message)
        {
            // Get the connection string from app settings
            string connectionString = _configuration["StorageConnectionString"];

            // Instantiate a QueueClient which will be used to create and manipulate the queue
            var queueClient = new QueueClient(connectionString, queueName, new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 });

            // Create the queue if it doesn't already exist
            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {
                // Send a message to the queue
                queueClient.SendMessage(message);
            }
        }
    }
}
