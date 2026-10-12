using System.ComponentModel.DataAnnotations;

namespace Miltha.Tech.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredBoolAttribute : RequiredAttribute
    {
        public RequiredBoolAttribute() => ErrorMessage = MessageErrorsExtensions.RequiredProperty;
    }
}