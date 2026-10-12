namespace System
{
    public static class CharExtensions
    {
        public static int ToIntExtended(this char currentValue, int defaultIntValue = 0)
        {
            if (int.TryParse(currentValue.ToString(), out var parsedInt))
                return parsedInt;

            return defaultIntValue;
        }
    }
}