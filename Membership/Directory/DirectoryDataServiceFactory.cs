using Manufacturing.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Microsoft.WindowsAzure.ActiveDirectory
{
    public class DirectoryDataServiceFactory
    {
        private static Dictionary<string,AADJWTToken> _tokenLookup = new Dictionary<string, AADJWTToken>();

        public static DirectoryDataService GetInstance(AuthenticationConfiguration config)
        {
            //get the tenantName based on logged in user
            string tenantName = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            AADJWTToken token;
            _tokenLookup.TryGetValue(tenantName, out token);

            if (token == null || token.WillExpireIn(1))
            {
                // Grab a new token

                // retrieve the clientId and password values from the Web.config file
                string clientId = config.ApiClientId;
                string password = config.ApiKey;

                // get a token using the helper, error handling???
                token = DirectoryDataServiceAuthorizationHelper.GetAuthorizationToken(tenantName, clientId, password);
                _tokenLookup[tenantName] = token;
            }

            // initialize a graphService instance using the token acquired from previous step
            DirectoryDataService graphService = new DirectoryDataService(tenantName, token);
            return graphService;
        }
    }
}