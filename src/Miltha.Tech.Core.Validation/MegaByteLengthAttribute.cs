using System.ComponentModel.DataAnnotations;

namespace Miltha.Tech.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MegaByteLengthAttribute(int maxLength) : ValidationAttribute(() => MessageErrorsExtensions.FileMaxLength)
    {
        public int MaxLength { get; } = maxLength;

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            var length = value.ToByte().Length / 1024 / 1024;

            return length <= MaxLength;
        }
    }
}