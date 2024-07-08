namespace loginresponsemodel
{
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class LoginResponseModel
    {
        [JsonProperty("tokenType")]
        public string TokenType { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("expiresIn")]
        public long ExpiresIn { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }

    public partial class LoginResponseModel
    {
        public static LoginResponseModel FromJson(string json) => JsonConvert.DeserializeObject<LoginResponseModel>(json, loginresponsemodel.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this LoginResponseModel self) => JsonConvert.SerializeObject(self, loginresponsemodel.Converter.Settings);
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
