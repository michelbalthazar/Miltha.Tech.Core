using System.ComponentModel.DataAnnotations;

namespace Miltha.Tech.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredEnumAttribute : RequiredAttribute
    {
        public RequiredEnumAttribute() => ErrorMessage = MessageErrorsExtensions.RequiredProperty;

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var type = value.GetType();
                return type.IsEnum && Enum.IsDefined(type, value);
            }
            else
                return false;
        }
    }
}