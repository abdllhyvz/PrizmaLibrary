namespace loginmodel
{
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class LoginModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public partial class LoginModel
    {
        public static LoginModel FromJson(string json) => JsonConvert.DeserializeObject<LoginModel>(json, loginmodel.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this LoginModel self) => JsonConvert.SerializeObject(self, loginmodel.Converter.Settings);
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
