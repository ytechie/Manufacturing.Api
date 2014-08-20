using Manufacturing.Api.Membership.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace Manufacturing.Api.Membership.Attributes
{
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Class, AllowMultiple = false)]
    public class AuthorizeMembershipInfoAccessAttribute : AuthorizeAttribute
    {
        public bool AllowSelf { get; set; }
        public string[] Roles { get; set; }
        
        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            // Ensure token is present
            if (!base.IsAuthorized(actionContext))
                return false;

            string userid = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            // a bit hacky, look for id in actions in user controller only
            if (AllowSelf)
            {
                object reqUserId;
                if (actionContext.ControllerContext.ControllerDescriptor.ControllerName == "User" &&
                    actionContext.ControllerContext.RouteData.Values.ContainsKey("id") &&
                    (string)actionContext.ControllerContext.RouteData.Values["id"] == userid)
                {
                    return true;
                }
            }

            // See if user is in one of the specified roles
            var repos = (IMembershipRepository)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IMembershipRepository));
            var user = repos.GetUser(userid);
            return user != null && user.Roles.Select(x => x.Name).Intersect(Roles).Any();
        }
    }
}