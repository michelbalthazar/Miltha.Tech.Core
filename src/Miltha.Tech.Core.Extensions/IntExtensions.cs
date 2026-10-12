namespace System
{
    public static class IntExtensions
    {
        public static T ToEnum<T>(this int currentValue)
        {
            var result = Enum.Parse(typeof(T), currentValue.ToString());

            if (((Enum)result).IsValid())
                return (T)result;

            return default;
        }
    }
}