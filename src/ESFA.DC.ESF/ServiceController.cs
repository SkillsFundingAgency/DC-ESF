﻿using System.Collections.Generic;
using System.Linq;
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
        private readonly IStorageController _storageController;

        public ServiceController(
            IFileHelper fileHelper,
            ITaskHelper taskHelper,
            IFileValidationService fileValidationService,
            IStorageController storageController,
            IReportingController reportingController)
        {
            _fileHelper = fileHelper;
            _taskHelper = taskHelper;
            _fileValidationService = fileValidationService;
            _reportingController = reportingController;
            _storageController = storageController;
        }

        public async Task RunTasks(
            IJobContextMessage jobContextMessage,
            IReadOnlyList<ITaskItem> tasks,
            CancellationToken cancellationToken)
        {
            var wrapper = new SupplementaryDataWrapper();
            var sourceFileModel = new SourceFileModel();
            if (tasks.SelectMany(t => t.Tasks).Contains(Constants.ValidationTask))
            {
                sourceFileModel = _fileHelper.GetSourceFileData(jobContextMessage);

                wrapper = await _fileValidationService.GetFile(sourceFileModel, cancellationToken);
                if (!wrapper.ValidErrorModels.Any())
                {
                    wrapper = _fileValidationService.RunFileValidators(sourceFileModel, wrapper);
                }

                if (wrapper.ValidErrorModels.Any())
                {
                    await _storageController.StoreValidationOnly(sourceFileModel, wrapper, cancellationToken);
                    await _reportingController.FileLevelErrorReport(wrapper, sourceFileModel, cancellationToken);
                    return;
                }
            }

            await _taskHelper.ExecuteTasks(tasks, sourceFileModel, wrapper, cancellationToken);
        }
    }
}
