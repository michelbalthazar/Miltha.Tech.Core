using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Miltha.Tech.Core.Data.Mongo.Serialization
{
    /// <summary>
    /// Persists <see cref="DateTimeOffset"/> as a UTC BSON Date (scalar int64) so that range
    /// queries, ordering and indexes remain efficient. Legacy array/documents ([Ticks, OffsetMinutes])
    /// written by the default driver serializer are normalized to UTC on read.
    /// </summary>
    public sealed class UtcDateTimeOffsetSerializer : SerializerBase<DateTimeOffset>
    {
        public override DateTimeOffset Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var value = BsonValueSerializer.Instance.Deserialize(context, args);

            if (value.IsBsonDateTime)
                return new DateTimeOffset(value.AsBsonDateTime.ToUniversalTime(), TimeSpan.Zero);

            if (value.IsBsonArray)
            {
                var array = value.AsBsonArray;

                if (array.Count < 2)
                    throw new FormatException("Invalid BSON array representation for DateTimeOffset.");

                var ticks = array[0].ToInt64();
                var offsetMinutes = array[1].ToInt64();

                return new DateTimeOffset(ticks, TimeSpan.Zero)
                    .ToOffset(TimeSpan.FromMinutes(offsetMinutes))
                    .ToUniversalTime();
            }

            if (value.IsString)
                return DateTimeOffset.Parse(value.AsString);

            throw new FormatException($"Unsupported BSON type '{value.BsonType}' for DateTimeOffset.");
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTimeOffset value)
        {
            context.Writer.WriteDateTime(BsonUtils.ToMillisecondsSinceEpoch(value.UtcDateTime));
        }
    }
}