using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FileLevel
{
    public class FileNameRule08 : IFileLevelValidator
    {
        private readonly IEsfRepository _esfRepository;

        public string ErrorMessage => "The date/time of the file is not greater than a previous transmission with the same ConRefNumber and UKPRN.";

        public bool IsValid { get; private set; }

        public bool RejectFile => true;

        public string ErrorName => "Filename_08";

        public bool IsWarning => false;

        public FileNameRule08(IEsfRepository esfRepository)
        {
            _esfRepository = esfRepository;
        }

        public async Task Execute(SourceFileModel sourceFileModel, SupplementaryDataModel model)
        {
            var previousFiles = await _esfRepository.PreviousFiles(sourceFileModel.UKPRN, sourceFileModel.ConRefNumber, CancellationToken.None);

            IsValid = previousFiles.All(f => f.DateTime <= sourceFileModel.PreparationDate);
        }
    }
}
