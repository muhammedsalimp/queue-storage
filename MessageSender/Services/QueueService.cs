using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace MessageSender.Services
{
    public class QueueService : IQueueService
    {
        private readonly IConfiguration _configuration;
        private ILogger _log;

        public QueueService(IConfiguration configuration, ILogger<QueueService> log)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this._log = log;
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

            _log.LogInformation($"Inserted: {message}");
        }
    }
}
