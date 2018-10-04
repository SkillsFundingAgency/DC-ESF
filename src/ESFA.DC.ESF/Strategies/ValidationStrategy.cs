using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Controllers;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Strategies
{
    public class ValidationStrategy : ITaskStrategy
    {
        private readonly IValidationController _controller;

        public ValidationStrategy(IValidationController controller)
        {
            _controller = controller;
        }

        public int Order => 1;

        public bool IsMatch(string taskName)
        {
            return taskName == Constants.ValidationTask;
        }

        public async Task Execute(
            SourceFileModel sourceFile,
            IList<SupplementaryDataModel> esfRecords, 
            IList<ValidationErrorModel> errors,
            CancellationToken cancellationToken)
        {
            foreach (var model in esfRecords)
            {
                await _controller.ValidateData(esfRecords, model);

                if (_controller.RejectFile)
                {
                    return;
                }

                foreach (var error in _controller.Errors)
                {
                    errors.Add(error);
                }
            }

            esfRecords = FilterOutInvalidRows(esfRecords, errors);
        }

        private IList<SupplementaryDataModel> FilterOutInvalidRows(IList<SupplementaryDataModel> models,
            IList<ValidationErrorModel> errors)
        {
            return models.Where(model => !errors.Any(e => e.ConRefNumber == model.ConRefNumber 
                                                          && e.DeliverableCode == model.DeliverableCode 
                                                          && e.CalendarYear == model.CalendarYear 
                                                          && e.CalendarMonth == model.CalendarMonth 
                                                          && e.ReferenceType == model.ReferenceType 
                                                          && e.Reference == model.Reference)).ToList();
        }
    }
}
