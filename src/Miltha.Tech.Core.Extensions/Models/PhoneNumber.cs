using Miltha.Tech.Core.Extensions.Enums;

namespace Miltha.Tech.Core.Extensions.Models
{
    public struct PhoneNumber(CountryCode Country, string Area, string Number)
    {
        public string Country { get; set; } = Country.ToIntAsString();
        public string Area { get; set; } = Area;
        public string Number { get; set; } = Number;
        public readonly string Full => GetFullPhoneNumber();

        private readonly string GetFullPhoneNumber() => $"{Country} {Area} {Number}";
    }
}