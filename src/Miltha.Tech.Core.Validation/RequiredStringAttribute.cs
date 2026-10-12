using System.ComponentModel.DataAnnotations;

namespace Miltha.Tech.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredStringAttribute : RequiredAttribute
    {
        public RequiredStringAttribute() => ErrorMessage = MessageErrorsExtensions.RequiredProperty;

        public override bool IsValid(object value)
        {
            if (value != null)
                return !string.IsNullOrWhiteSpace(value.ToString());
            else
                return false;
        }
    }
}