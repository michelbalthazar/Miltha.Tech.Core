using Miltha.Tech.Core.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Miltha.Tech.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredCnpjAttribute : RequiredAttribute
    {
        public RequiredCnpjAttribute() => ErrorMessage = MessageErrorsExtensions.RequiredProperty;

        public override bool IsValid(object value) =>
            value != null && BrazilDocumentValidator.IsValidCnpj(value.ToString());
    }
}