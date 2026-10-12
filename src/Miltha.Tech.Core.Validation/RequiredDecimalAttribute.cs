using System.ComponentModel.DataAnnotations;

namespace Miltha.Tech.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredDecimalAttribute : RequiredAttribute
    {
        public RequiredDecimalAttribute() => ErrorMessage = MessageErrorsExtensions.RequiredProperty;

        public override bool IsValid(object value)
        {
            if (value != null && decimal.TryParse(value.ToString(), out var result))
                return result >= 0;
            else
                return false;
        }
    }
}