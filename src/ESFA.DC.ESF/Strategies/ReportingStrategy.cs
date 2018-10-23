using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Controllers;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Strategies
{
    public class ReportingStrategy : ITaskStrategy
    {
        private readonly IReportingController _reportingController;

        public int Order => 2;

        public bool IsMatch(string taskName)
        {
            return taskName == Constants.ReportingTask;
        }

        public ReportingStrategy(IReportingController reportingController)
        {
            _reportingController = reportingController;
        }

        public async Task Execute(
            SourceFileModel sourceFile, 
            SupplementaryDataWrapper supplementaryDataWrapper, 
            CancellationToken cancellationToken)
        {
            await _reportingController.ProduceReports(supplementaryDataWrapper, sourceFile, cancellationToken);
        }
    }
}
