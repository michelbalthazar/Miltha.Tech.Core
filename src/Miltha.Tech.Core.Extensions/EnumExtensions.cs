namespace System
{
    public static class EnumExtensions
    {
        public static int ToIntExtended(this Enum enumValue)
            => Convert.ToInt32(enumValue);

        public static string ToIntAsString(this Enum enumValue)
            => Convert.ToInt32(enumValue).ToString();

        public static bool IsValid(this Enum enumValue)
            => Enum.IsDefined(enumValue.GetType(), enumValue);
    }
}