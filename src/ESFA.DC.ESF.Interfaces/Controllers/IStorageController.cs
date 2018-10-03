﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Controllers
{
    public interface IStorageController
    {
        Task<bool> StoreData(
            SourceFileModel sourceFile,
            IEnumerable<SupplementaryDataModel> models,
            CancellationToken cancellationToken);
    }
}