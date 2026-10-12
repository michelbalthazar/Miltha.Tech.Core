namespace Miltha.Tech.Core.Extensions
{
    /// <summary>
    /// Self-contained Brazilian document (CPF/CNPJ) validation, without external dependencies.
    /// </summary>
    public static class BrazilDocumentValidator
    {
        /// <summary>
        /// Validates a Brazilian CPF (Cadastro de Pessoas Físicas).
        /// </summary>
        public static bool IsValidCpf(string cpf)
        {
            var digits = ToDigits(cpf);

            if (digits.Length != 11 || AllSame(digits))
                return false;

            var first = ComputeCheckDigit(digits, 9, 10);
            var second = ComputeCheckDigit(digits, 10, 11);

            return digits[9] == first && digits[10] == second;
        }

        /// <summary>
        /// Validates a Brazilian CNPJ (Cadastro Nacional da Pessoa Jurídica).
        /// Since 2026 CNPJ accepts alphanumeric characters (uppercase letters), which are
        /// converted to numeric values (A = 10 ... Z = 35) before the check digits are computed.
        /// </summary>
        public static bool IsValidCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            var values = ToAlphaNumericValues(cnpj);

            if (values.Length != 14 || AllSame(values))
                return false;

            var first = ComputeCheckDigit(values, 12, 5);
            var second = ComputeCheckDigit(values, 13, 6);

            return values[12] == first && values[13] == second;
        }

        private static int[] ToDigits(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return [];

            var list = new List<int>(value.Length);

            foreach (var c in value)
            {
                if (char.IsDigit(c))
                    list.Add(c - '0');
            }

            return list.ToArray();
        }

        private static int[] ToAlphaNumericValues(string value)
        {
            var list = new List<int>(value.Length);

            foreach (var c in value)
            {
                if (c >= '0' && c <= '9')
                    list.Add(c - '0');
                else if (c >= 'A' && c <= 'Z')
                    list.Add(c - 'A' + 10);
                else if (c >= 'a' && c <= 'z')
                    list.Add(c - 'a' + 10);
            }

            return list.ToArray();
        }

        private static bool AllSame(int[] digits)
        {
            for (var i = 1; i < digits.Length; i++)
            {
                if (digits[i] != digits[0])
                    return false;
            }

            return true;
        }

        private static int ComputeCheckDigit(int[] digits, int count, int startWeight)
        {
            var sum = 0;
            var weight = startWeight;

            for (var i = 0; i < count; i++)
            {
                sum += digits[i] * weight;
                weight = weight == 2 ? 9 : weight - 1;
            }

            var rest = sum % 11;

            return rest < 2 ? 0 : 11 - rest;
        }
    }
}