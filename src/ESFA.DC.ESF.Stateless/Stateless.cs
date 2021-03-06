﻿using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.JobContextManager.Interface;
using ESFA.DC.JobContextManager.Model;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ESFA.DC.ESF.Service.Stateless
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    public class Stateless : StatelessService
    {
        private readonly IJobContextManager<JobContextMessage> _jobContextManager;

        public Stateless(StatelessServiceContext context, IJobContextManager<JobContextMessage> jobContextManager)
            : base(context)
        {
            _jobContextManager = jobContextManager;
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            bool initialised = false;
            try
            {
                _jobContextManager.OpenAsync(cancellationToken);
                initialised = true;
                await Task.Delay(Timeout.Infinite, cancellationToken);
            }
            catch
            {
                // Ignore, as an exception is only really thrown on cancellation of the token.
            }
            finally
            {
                if (initialised)
                {
                    await _jobContextManager.CloseAsync();
                }
            }
        }
    }
}
