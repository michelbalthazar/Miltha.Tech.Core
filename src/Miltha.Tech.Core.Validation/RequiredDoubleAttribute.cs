using System.ComponentModel.DataAnnotations;

namespace Miltha.Tech.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredDoubleAttribute : RequiredAttribute
    {
        public RequiredDoubleAttribute() => ErrorMessage = MessageErrorsExtensions.RequiredProperty;

        public override bool IsValid(object value)
        {
            if (value != null && double.TryParse(value.ToString(), out var result))
                return result >= 0;
            else
                return false;
        }
    }
}