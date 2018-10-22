namespace ESFA.DC.ESF.Service.Config
{
    public sealed class ServiceBusOptions
    {
        public string AuditQueueName { get; set; }

        public string JobStatusQueueName { get; set; }

        public string ServiceBusConnectionString { get; set; }

        public string TopicName { get; set; }

        public string SubscriptionName { get; set; }
    }
}
