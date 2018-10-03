using System;
using ESFA.DC.Queueing;

namespace ESFA.DC.ESF.Service.Config
{
    public class ServiceBusQueueConfig : QueueConfiguration
    {
        public ServiceBusQueueConfig(string connectionString, string queueName, int maxConcurrentCalls, int minimumBackoffSeconds = 5, int maximumBackoffSeconds = 50, int maximumRetryCount = 10, int maximumCallbackTimeoutMinutes = 10) 
            : base(connectionString, queueName, maxConcurrentCalls, minimumBackoffSeconds, maximumBackoffSeconds, maximumRetryCount, TimeSpan.FromMinutes(maximumCallbackTimeoutMinutes))
        {
        }
    }
}
