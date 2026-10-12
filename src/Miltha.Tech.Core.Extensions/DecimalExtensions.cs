namespace System
{
    public static class DecimalExtensions
    {
        public static decimal ToDecimal(this double value) =>
            Convert.ToDecimal(value);

        public static decimal SafeDivision(this decimal numerator, decimal denominator) =>
            denominator > 0 ? numerator / denominator : 0;
    }
}