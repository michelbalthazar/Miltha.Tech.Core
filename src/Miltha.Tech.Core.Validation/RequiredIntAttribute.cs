using System.ComponentModel.DataAnnotations;

namespace Miltha.Tech.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredIntAttribute : RequiredAttribute
    {
        public RequiredIntAttribute() => ErrorMessage = MessageErrorsExtensions.RequiredProperty;

        public override bool IsValid(object value)
        {
            if (value != null && int.TryParse(value.ToString(), out var result))
                return result >= 0;
            else
                return false;
        }
    }
}