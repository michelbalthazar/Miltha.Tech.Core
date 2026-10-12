namespace System
{
    public static class GuidExtensions
    {
        public static bool IsNullOrEmpty(this Guid? value) =>
            value == null || value == Guid.Empty;

        public static bool IsEmpty(this Guid value) =>
            value == Guid.Empty;
    }
}