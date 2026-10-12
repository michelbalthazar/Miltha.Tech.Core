using Miltha.Tech.Core.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Miltha.Tech.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredDateAttribute : RequiredAttribute
    {
        public RequiredDateAttribute() => ErrorMessage = MessageErrorsExtensions.RequiredProperty;

        public override bool IsValid(object value)
        {
            if (value != null)
                return value.ToString().ToDateTime() > DateTime.MinValue;
            else
                return false;
        }
    }
}