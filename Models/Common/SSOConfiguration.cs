namespace ReStudyAPI.Models.Common
{
    public class SSOConfiguration
    {
        public string BaseUrl { get; set; }
        public string TokenPath { get; set; }
        public string UserInfoPath { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
