namespace ESFA.DC.ESF.Service.Stateless.Config
{
    public sealed class ServiceBusOptions
    {
        public string AuditQueueName { get; set; }

        public string ServiceBusConnectionString { get; set; }

        public string TopicName { get; set; }

        public string ReportingSubscriptionName { get; set; }
    }
}
