namespace registerresponsemodel
{
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class RegisterResponseModel
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }

        [JsonProperty("instance")]
        public string Instance { get; set; }

        [JsonProperty("errors")]
        public Errors Errors { get; set; }

        [JsonProperty("additionalProp1")]
        public string AdditionalProp1 { get; set; }

        [JsonProperty("additionalProp2")]
        public string AdditionalProp2 { get; set; }

        [JsonProperty("additionalProp3")]
        public string AdditionalProp3 { get; set; }
    }

    public partial class Errors
    {
        [JsonProperty("additionalProp1")]
        public List<string> AdditionalProp1 { get; set; }

        [JsonProperty("additionalProp2")]
        public List<string> AdditionalProp2 { get; set; }

        [JsonProperty("additionalProp3")]
        public List<string> AdditionalProp3 { get; set; }
    }

    public partial class RegisterResponseModel
    {
        public static RegisterResponseModel FromJson(string json) => JsonConvert.DeserializeObject<RegisterResponseModel>(json, registerresponsemodel.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this RegisterResponseModel self) => JsonConvert.SerializeObject(self, registerresponsemodel.Converter.Settings);
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
