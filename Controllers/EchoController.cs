using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace Manufacturing.Api.Controllers
{
    public class EchoController : ApiController
    {
        public string Get(string message)
        {
            return message;
        }

        // This is here for authorization testing
        [Authorize]
        public string Get(bool whoAmI)
        {
            if (ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/scope").Value != "user_impersonation")
            {
                throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, ReasonPhrase = "The Scope claim does not contain 'user_impersonation' or scope claim not found" });
            }

            Claim subject = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier);

            return subject.Value;
        }
    }
}
