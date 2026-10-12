namespace Miltha.Tech.Core.Extensions.Enums
{
    public enum PersonDocumentType
    {
        None = 0,

        /// <summary>
        /// CPF
        /// </summary>
        SocialSecurityNumber = 2,

        /// <summary>
        /// CNPJ
        /// </summary>
        TaxNumber = 3,

        /// <summary>
        /// Inscricao Estadual
        /// </summary>
        TaxNumberFederal = 4,

        /// <summary>
        /// Inscricao Municipal
        /// </summary>
        TaxNumberTown = 5,
    }
}