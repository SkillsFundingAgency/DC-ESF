using ESFA.DC.ESF.Service.Config;
using ESFA.DC.ServiceFabric.Helpers.Interfaces;

namespace ESFA.DC.ILR1819.DataStore.Stateless.Test
{
    public sealed class TestConfigurationHelper : IConfigurationHelper
    {
        public T GetSectionValues<T>(string sectionName)
        {
            switch (sectionName)
            {
                case "VersionSection":
                    return (T) (object) new VersionInfo
                    {
                        ServiceReleaseVersion = "1.2.3.4"
                    };

                case "DataStoreSection":
                    return (T)(object)new PersistDataConfiguration()
                    {
                        DataStoreConnectionString = "Server=.;Database=myDataBase;User Id=myUsername;Password = myPassword;"
                    };
                //case "TopicAndTaskSection":
                //    return (T)GetTopicsAndTasks();
                case "AzureStorageSection":
                    return (T)(object)new AzureStorageOptions()
                    {
                        AzureBlobConnectionString = "AzureBlobConnectionString",
                        AzureBlobContainerName = "AzureBlobContainerName"
                    };
                case "ServiceBusSettings":
                    return (T)(object)new ServiceBusOptions()
                    {
                        AuditQueueName = "AuditQueueName",
                        ServiceBusConnectionString = "ServiceBusConnectionString",
                        TopicName = "TopicName",
                        SubscriptionName = "DataStore"
                    };
                case "LoggerSection":
                    return (T)(object)new LoggerOptions
                    {
                        LoggerConnectionString = "Server=.;Database=myDataBase;User Id=myUsername;Password = myPassword;"
                    };
                case "ILR1819Section":
                    return (T)(object)new IRL1819Configuration
                    {
                        ILR1819ConnectionString = "Server=.;Database=myDataBase;User Id=myUsername;Password = myPassword;"
                    };
                case "ESFSection":
                    return (T)(object)new ESFConfiguration
                    {
                        ESFConnectionString = "Server=.;Database=myDataBase;User Id=myUsername;Password = myPassword;"
                    };
                case "JobStatusSection":
                    return (T) (object) new JobStatusQueueOptions();
                case "ReferenceDataSection":
                    return (T)(object)new ReferenceDataConfig
                    {
                        LARSConnectionString = "Server=.;Database=myDataBase;User Id=myUsername;Password = myPassword;",
                        OrganisationConnectionString = "Server=.;Database=myDataBase;User Id=myUsername;Password = myPassword;",
                        PostcodesConnectionString = "Server=.;Database=myDataBase;User Id=myUsername;Password = myPassword;"
                    };
            }

            return default(T);
        }

        //public static ITopicAndTaskSectionOptions GetTopicsAndTasks()
        //{
        //    return new TopicAndTaskSectionOptions()
        //    {
        //        TopicReports_TaskGenerateAllbOccupancyReport = "TopicReports_TaskGenerateAllbOccupancyReport",
        //        TopicReports_TaskGenerateValidationReport = "TopicReports_TaskGenerateValidationReport",
        //        TopicReports_TaskGenerateFundingSummaryReport = "TopicReports_TaskGenerateFundingSummaryReport",
        //        TopicDeds = "TopicDeds",
        //        TopicDeds_TaskPersistDataToDeds = "TopicDeds_TaskPersistDataToDeds",
        //        TopicFunding = "TopicFunding",
        //        TopicReports = "TopicReports",
        //        TopicReports_TaskGenerateMainOccupancyReport = "TopicReports_TaskGenerateMainOccupancyReport",
        //        TopicReports_TaskGenerateSummaryOfFunding1619Report = "TopicReports_TaskGenerateSummaryOfFunding1619Report",
        //        TopicValidation = "TopicValidation",
        //        TopicReports_TaskGenerateMathsAndEnglishReport = "TopicReports_TaskGenerateMathsAndEnglishReport"
        //    };
        //}
    }
}
