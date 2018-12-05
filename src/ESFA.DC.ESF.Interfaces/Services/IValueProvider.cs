using System.Collections.Generic;

namespace ESFA.DC.ESF.Interfaces.Services
{
    public interface IValueProvider
    {
        void GetFormattedValue(List<object> values, object value);
    }
}
