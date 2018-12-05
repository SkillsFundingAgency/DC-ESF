using System;
using System.Collections.Generic;
using System.Threading;
using Autofac;
using Autofac.Integration.ServiceFabric;
using ESFA.DC.ESF.Interfaces.Controllers;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.JobContextManager.Model.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.ServiceFabric.Helpers;
using Xunit;

namespace ESFA.DC.ESF.Tests
{
    public class EndToEndTest
    {
        [Fact]
        public void TestEntryPoint()
        {
            //var builder = DIComposition.BuildContainer(new ConfigurationHelper());

            //builder.RegisterServiceFabricSupport();

            //// Register the stateless service.
            //builder.RegisterStatelessService<Stateless>("ESFA.DC.ESF.Service.StatelessType");

            //using (var container = builder.Build())
            //{
            //    var serviceController = container.Resolve<IServiceController>();
            //    var logger = container.Resolve<ILogger>();
            //    EntryPoint ep = new EntryPoint(serviceController, logger);

            //    JobContextMessage jobContextMessage = BuildJobContextMessage();

            //    var result = ep.Callback(jobContextMessage, CancellationToken.None);

            //    Assert.True(result.Result);
            //}
        }

        private JobContextMessage BuildJobContextMessage()
        {
            return new JobContextMessage
            {
                JobId = 30007,
                SubmissionDateTimeUtc = DateTime.Now,
                TopicPointer = 0,
                KeyValuePairs = new Dictionary<string, object>
                {
                    { "UkPrn", "10034309" },
                    { "Container", "esf-files" },
                    { "Filename", "SUPPDATA-10034309-ESF-2270-20180909-090911.csv" }
                },
                Topics = new List<ITopicItem>
                {
                    new TopicItem
                    {
                        SubscriptionName = "Process",
                        SubscriptionSqlFilterValue = "Process",
                        Tasks = new List<ITaskItem>
                        {
                            new TaskItem
                            {
                                Tasks = new List<string>
                                {
                                    "Validation",
                                    "Storage",
                                    "Reporting"
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
