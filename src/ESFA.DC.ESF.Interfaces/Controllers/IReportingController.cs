﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Controllers
{
    public interface IReportingController
    {
        Task FileLevelErrorReport(
            IList<SupplementaryDataModel> models,
            IList<ValidationErrorModel> errors,
            SourceFileModel sourceFile,
            CancellationToken cancellationToken);
    }
}