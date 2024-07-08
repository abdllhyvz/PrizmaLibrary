namespace rolesmodel
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class RolesModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("normalizedName")]
        public string NormalizedName { get; set; }

        [JsonProperty("concurrencyStamp")]
        public object ConcurrencyStamp { get; set; }
    }

    public partial class RolesModel
    {
        public static List<RolesModel> FromJson(string json) => JsonConvert.DeserializeObject<List<RolesModel>>(json, rolesmodel.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this List<RolesModel> self) => JsonConvert.SerializeObject(self, rolesmodel.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
