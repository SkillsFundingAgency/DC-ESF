using System.Reflection;

namespace ESFA.DC.ESF.Models.Generation
{
    public sealed class ModelProperty
    {
        public ModelProperty(object name, PropertyInfo methodInfo)
        {
            Name = name;
            MethodInfo = methodInfo;
        }

        public object Name { get; }

        public PropertyInfo MethodInfo { get; }
    }
}
