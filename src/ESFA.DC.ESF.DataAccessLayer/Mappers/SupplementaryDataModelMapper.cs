﻿using ESFA.DC.ESF.Database.EF;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.DataAccessLayer.Mappers
{
    public class SupplementaryDataModelMapper : ISupplementaryDataModelMapper
    {
        public SupplementaryDataModel GetModelFromEntity(SupplementaryData entity)
        {
            return new SupplementaryDataModel
            {
                DeliverableCode = entity.DeliverableCode,
                ConRefNumber = entity.ConRefNumber,
                ULN = entity.ULN,
                CostType = entity.CostType,
                CalendarYear = entity.CalendarYear,
                CalendarMonth = entity.CalendarMonth,
                ReferenceType = entity.ReferenceType,
                Reference = entity.Reference,
                ProviderSpecifiedReference = entity.ProviderSpecifiedReference,
                StaffName = entity.StaffName,
                OrgHours = entity.OrgHours,
                ProjectHours = entity.ProjectHours,
                HourlyRate = entity.HourlyRate,
                TotalHoursWorked = entity.TotalHoursWorked,
                Value = entity.Value
            };
        }

        public SupplementaryDataModel GetSupplementaryDataModelFromLooseModel(SupplementaryDataLooseModel looseModel)
        {
            return new SupplementaryDataModel
            {
                ConRefNumber = looseModel.ConRefNumber,
                ULN = ConvertToNullableLong(looseModel.ULN),
                DeliverableCode = looseModel.DeliverableCode,
                CostType = looseModel.CostType,
                ReferenceType = looseModel.ReferenceType,
                Reference = looseModel.Reference,
                ProviderSpecifiedReference = looseModel.ProviderSpecifiedReference,
                CalendarMonth = ConvertToNullableInt(looseModel.CalendarMonth),
                CalendarYear = ConvertToNullableInt(looseModel.CalendarYear),
                StaffName = looseModel.StaffName,
                OrgHours = ConvertToNullableInt(looseModel.OrgHours),
                ProjectHours = ConvertToNullableInt(looseModel.ProjectHours),
                HourlyRate = ConvertToNullableInt(looseModel.HourlyRate),
                TotalHoursWorked = ConvertToNullableInt(looseModel.TotalHoursWorked),
                Value = ConvertToNullableInt(looseModel.Value)
            };
        }

        private long? ConvertToNullableLong(string value)
        {
            return value == null ? (long?)null : long.Parse(value);
        }

        private int? ConvertToNullableInt(string value)
        {
            return value == null ? (int?)null : int.Parse(value);
        }
    }
}