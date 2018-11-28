using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Reports.Services;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ReportingService.Services
{
    public class SupplementaryDataService : ISupplementaryDataService
    {
        private readonly IEsfRepository _repository;

        private readonly ISourceFileModelMapper _fileModelMapper;

        private readonly ISupplementaryDataModelMapper _supplementaryDataMapper;

        public SupplementaryDataService(
            IEsfRepository repository,
            ISourceFileModelMapper fileModelMapper,
            ISupplementaryDataModelMapper supplementaryDataMapper)
        {
            _repository = repository;
            _fileModelMapper = fileModelMapper;
            _supplementaryDataMapper = supplementaryDataMapper;
        }

        public async Task<IList<SourceFileModel>> GetPreviousContractImportFilesForProvider(
            string ukPrn,
            CancellationToken cancellationToken)
        {
            var existingData = new List<SourceFileModel>();

            var contractNumbers = await _repository.GetContractsForProvider(ukPrn, cancellationToken);

            foreach (var contractNumber in contractNumbers)
            {
                var file = await _repository.PreviousFiles(ukPrn, contractNumber, cancellationToken);
                if (file == null)
                {
                    continue;
                }

                existingData.Add(_fileModelMapper.GetModelFromEntity(file));
            }

            return existingData;
        }

        public async Task<IList<SupplementaryDataModel>> GetSupplementaryDataPerSourceFile(
            int sourceFileId,
            CancellationToken cancellationToken)
        {
            var supplementaryData = await _repository.GetSupplementaryDataPerSourceFile(sourceFileId, cancellationToken);

            return supplementaryData.Select(data => _supplementaryDataMapper.GetModelFromEntity(data)).ToList();
        }
    }
}