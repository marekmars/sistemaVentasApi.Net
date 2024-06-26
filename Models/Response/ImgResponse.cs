// Generated by https://quicktype.io

namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Text.Json;
    using System.Text.Json.Serialization;

    public partial class ImgResponse
    {
        [JsonPropertyName("status")]
        public long Status { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("deletehash")]
        public string Deletehash { get; set; }

        [JsonPropertyName("account_id")]
        public object AccountId { get; set; }

        [JsonPropertyName("account_url")]
        public object AccountUrl { get; set; }

        [JsonPropertyName("ad_type")]
        public object AdType { get; set; }

        [JsonPropertyName("ad_url")]
        public object AdUrl { get; set; }

        [JsonPropertyName("title")]
        public object Title { get; set; }

        [JsonPropertyName("description")]
        public object Description { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("width")]
        public long Width { get; set; }

        [JsonPropertyName("height")]
        public long Height { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("views")]
        public long Views { get; set; }

        [JsonPropertyName("section")]
        public object Section { get; set; }

        [JsonPropertyName("vote")]
        public object Vote { get; set; }

        [JsonPropertyName("bandwidth")]
        public long Bandwidth { get; set; }

        [JsonPropertyName("animated")]
        public bool Animated { get; set; }

        [JsonPropertyName("favorite")]
        public bool Favorite { get; set; }

        [JsonPropertyName("in_gallery")]
        public bool InGallery { get; set; }

        [JsonPropertyName("in_most_viral")]
        public bool InMostViral { get; set; }

        [JsonPropertyName("has_sound")]
        public bool HasSound { get; set; }

        [JsonPropertyName("is_ad")]
        public bool IsAd { get; set; }

        [JsonPropertyName("nsfw")]
        public object Nsfw { get; set; }

        [JsonPropertyName("link")]
        public Uri Link { get; set; }

        [JsonPropertyName("tags")]
        public object[] Tags { get; set; }

        [JsonPropertyName("datetime")]
        public long Datetime { get; set; }

        [JsonPropertyName("mp4")]
        public string Mp4 { get; set; }

        [JsonPropertyName("hls")]
        public string Hls { get; set; }
    }

    public partial class ImgResponse
    {
        public static ImgResponse FromJson(string json) => JsonSerializer.Deserialize<ImgResponse>(json, QuickType.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ImgResponse self) => JsonSerializer.Serialize(self, QuickType.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerOptions Settings = new(JsonSerializerDefaults.Web)
        {
            Converters =
            {
                new DateOnlyConverter(),
                new TimeOnlyConverter(),
            },
        };
    }
    
    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        private readonly string serializationFormat;
        public DateOnlyConverter() : this(null) { }

        public DateOnlyConverter(string? serializationFormat)
        {
            this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";
        }

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return DateOnly.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(serializationFormat));
    }
    public class TimeOnlyConverter : JsonConverter<TimeOnly>
    {
        private readonly string serializationFormat;

        public TimeOnlyConverter() : this(null) { }

        public TimeOnlyConverter(string? serializationFormat)
        {
            this.serializationFormat = serializationFormat ?? "HH:mm:ss.fff";
        }

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return TimeOnly.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(serializationFormat));
    }
}
