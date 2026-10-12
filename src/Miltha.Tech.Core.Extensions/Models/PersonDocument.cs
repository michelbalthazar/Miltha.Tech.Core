using Miltha.Tech.Core.Extensions.Enums;

namespace Miltha.Tech.Core.Extensions
{
    public struct PersonDocument
    {
        public string Number { get; set; }
        public PersonDocumentType Type { get; set; }

        public PersonDocument(string number)
        {
            Number = number;
            Type = GetDocumentType(number);
        }

        public static PersonDocumentType GetDocumentType(string number)
        {
            if (BrazilDocumentValidator.IsValidCnpj(number))
                return PersonDocumentType.TaxNumber;
            else if (BrazilDocumentValidator.IsValidCpf(number))
                return PersonDocumentType.SocialSecurityNumber;
            else
                return PersonDocumentType.None;
        }
    }
}