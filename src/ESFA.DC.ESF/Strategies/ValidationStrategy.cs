using System.Collections.Generic;
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

        public bool IsMatch(string taskName)
        {
            return taskName == string.Empty;
        }

        public async Task Execute(IList<ESFModel> esfRecords)
        {
            foreach (var model in esfRecords)
            {
                await _controller.ValidateData(model);
            }
        }
    }
}
