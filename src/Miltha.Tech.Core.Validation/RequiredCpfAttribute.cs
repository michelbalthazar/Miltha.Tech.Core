using Miltha.Tech.Core.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Miltha.Tech.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredCpfAttribute : RequiredAttribute
    {
        public RequiredCpfAttribute() => ErrorMessage = MessageErrorsExtensions.RequiredProperty;

        public override bool IsValid(object value) =>
            value != null && BrazilDocumentValidator.IsValidCpf(value.ToString());
    }
}