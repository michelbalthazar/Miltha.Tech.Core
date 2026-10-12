using Miltha.Tech.Core.Extensions.Enums;
using System.Text.Json.Serialization;

namespace Miltha.Tech.Core.Extensions
{
    public abstract record FileBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MediaType MediaType { get; set; }

        public string Name { get; set; }

        public byte[] File { get; set; }
    }
}