using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Controllers;
using ESFA.DC.ESF.Interfaces.Helpers;
using ESFA.DC.ESF.Interfaces.Services;
using ESFA.DC.ESF.Models;
using ESFA.DC.JobContextManager.Model.Interface;

namespace ESFA.DC.ESF
{
    public class ServiceController : IServiceController
    {
        private readonly IFileHelper _fileHelper;
        private readonly ITaskHelper _taskHelper;
        private readonly IFileValidationService _fileValidationService;
        private readonly IReportingController _reportingController;

        public ServiceController(
            IFileHelper fileHelper,
            ITaskHelper taskHelper,
            IFileValidationService fileValidationService,
            IReportingController reportingController)
        {
            _fileHelper = fileHelper;
            _taskHelper = taskHelper;
            _fileValidationService = fileValidationService;
            _reportingController = reportingController;
        }

        public async Task RunTasks(IJobContextMessage jobContextMessage,
            IReadOnlyList<ITaskItem> tasks,
            CancellationToken cancellationToken)
        {
            IList<SupplementaryDataModel> esfRecords = new List<SupplementaryDataModel>();
            IList<ValidationErrorModel> errors = new List<ValidationErrorModel>();

            var sourceFileModel = _fileHelper.GetSourceFileData(jobContextMessage);

            var validSchema = await _fileValidationService.GetFile(sourceFileModel, esfRecords, errors, cancellationToken);
            var passedFileValidation = await _fileValidationService.RunFileValidators(sourceFileModel, esfRecords, errors);

            if (validSchema && passedFileValidation)
            {
                await _taskHelper.ExecuteTasks(tasks, sourceFileModel, esfRecords, cancellationToken);

                return;
            }

            await _reportingController.FileLevelErrorReport(esfRecords, errors, sourceFileModel, cancellationToken);
        }
    }
}
