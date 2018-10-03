using ESFA.DC.ESF.Models;
using System;

namespace ESFA.DC.ESF.DataStore.Tests.Builders
{
    public class SourceFileModelBuilder
    {
        public static SourceFileModel BuildSourceFileModel()
        {
            return new SourceFileModel
            {
                ConRefNumber = "1234567890abcdefghij",
                UKPRN = "12345678",
                FileName = "foo.csv",
                PreparationDate = DateTime.Now.AddDays(-1),
                SuppliedDate = DateTime.Now
            };
        }
    }
}
