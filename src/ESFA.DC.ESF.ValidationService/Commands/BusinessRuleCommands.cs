using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands
{
    public class BusinessRuleCommands : IValidatorCommand
    {
        private readonly IList<IBusinessRuleValidator> _validators;

        public bool IsValid { get; private set; }

        public bool RejectFile { get; private set; }    

        public BusinessRuleCommands(IList<IBusinessRuleValidator> validators)
        {
            _validators = validators;
        }

        public async Task Execute(ESFModel model)
        {
            await Task.Run(() => Parallel.ForEach(_validators, v => v.Execute(model)));
            //foreach (var validator in _validators)
            //{
            //    await validator.Execute(model);
            //}

            var failed = _validators.Where(v => !v.IsValid);
            if (failed.Any())
            {

            }
        }
    }
}