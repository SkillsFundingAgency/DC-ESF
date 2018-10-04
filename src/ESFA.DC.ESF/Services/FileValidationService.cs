using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using ESFA.DC.ESF.Interfaces.Helpers;
using ESFA.DC.ESF.Interfaces.Services;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ESF.Services
{
    public class FileValidationService : IFileValidationService
    {
        private readonly IList<IFileLevelValidator> _validators;

        private readonly IFileHelper _fileHelper;

        private readonly ILogger _logger;

        public FileValidationService(IList<IFileLevelValidator> validators,
            IFileHelper fileHelper,
            ILogger logger)
        {
            _fileHelper = fileHelper;
            _validators = validators;
            _logger = logger;
        }

        public async Task<bool> GetFile(
            SourceFileModel sourceFileModel,
            IList<SupplementaryDataModel> esfRecords,
            IList<ValidationErrorModel> errors,
            CancellationToken cancellationToken)
        {
            try
            {
                esfRecords = await _fileHelper.GetESFRecords(sourceFileModel, cancellationToken);
                if (esfRecords == null || !esfRecords.Any())
                {
                    _logger.LogInfo("No ESF records to process");
                }
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"The file format is incorrect, key: {sourceFileModel.FileName}", ex);

                errors.Add(new ValidationErrorModel
                {
                    RuleName = "Fileformat_01",
                    ErrorMessage = "The file format is incorrect. Please check the field headers are as per the Guidance document.",
                    IsWarning = false
                });
                return false;
            }

            return true;
        }

        public async Task<bool> RunFileValidators(SourceFileModel sourceFileModel,
            IList<SupplementaryDataModel> models,
            IList<ValidationErrorModel> errors)
        {
            foreach (var model in models)
            {
                foreach (var validator in _validators)
                {
                    await validator.Execute(sourceFileModel, model);

                    if (validator.IsValid)
                    {
                        continue;
                    }

                    errors.Add(new ValidationErrorModel
                    {
                        RuleName = validator.ErrorName,
                        ErrorMessage = validator.ErrorMessage,
                        IsWarning = false
                    });
                    return false;
                }
            }

            return true;
        }
    }
}
