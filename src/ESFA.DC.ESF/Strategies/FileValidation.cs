using System;
using System.Collections.Generic;
using System.Text;
using ESFA.DC.ESF.Interfaces.Validation;

namespace ESFA.DC.ESF.Strategies
{
    public class FileValidation
    {
        private readonly IList<IFileLevelValidator> _validators;

        public FileValidation(IList<IFileLevelValidator> validators)
        {
            _validators = validators;
        }

        public void Execute()
        {
            foreach (var validator in _validators)
            {
                // validator.Execute()
            }
        }
    }
}
