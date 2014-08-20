namespace Manufacturing.Api
{
    public class AuthenticationConfiguration
    {
        public bool RequireAuthentication { get; set; }
        public string DirectoryDomain { get; set; }
        public string AppName { get; set; }
        public string AppRedirectUri { get; set; }
        public string AppClientId { get; set; }
        public string ApiAppName { get; set; }
        public string ApiAppSignOnUrl { get; set; }
        public string ApiAppId { get; set; }
        public string ApiClientId { get; set; } // needed for graph api use
        public string ApiKey { get; set; } // needed for graph api use
    }
}