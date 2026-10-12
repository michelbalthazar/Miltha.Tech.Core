using System.ComponentModel.DataAnnotations;

namespace Miltha.Tech.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredObjectAttribute : RequiredAttribute
    {
        public RequiredObjectAttribute() => ErrorMessage = MessageErrorsExtensions.RequiredProperty;

        public override bool IsValid(object value) =>
            value != null;
    }
}