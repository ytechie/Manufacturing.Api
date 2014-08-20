using Manufacturing.Api.Membership.Attributes;
using Manufacturing.Api.Membership.Models;
using Manufacturing.Api.Membership.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    [AuthorizeMembershipInfoAccess(Roles = new[] { "Administrators" })]
    public class UserController : ApiController
    {
        private IMembershipRepository _repos;

        public UserController(IMembershipRepository repos)
        {
            _repos = repos;
        }

        // GET api/user
        public IEnumerable<UserSummary> Get()
        {
            return _repos.GetAllUsers();
        }

        // GET api/user/5abc
        // Alter auth for this method to let users retrieve their own info...
        [AuthorizeMembershipInfoAccess(AllowSelf = true, Roles = new[] { "Administrators" })]
        public UserDetail Get(string id)
        {
            return _repos.GetUser(id);
        }

        // POST api/user
        public HttpResponseMessage Post(UserToCreate user)
        {
            var result = _repos.CreateUser(user);
            if (result == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            // Grab the user info to include in the response
            var response = Request.CreateResponse(HttpStatusCode.Created, result);

            string uri = Url.Link("DefaultApi", new { id = result.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // PUT api/user/5abc
        public void Put(string id, UserToUpdate user)
        {
            var result = _repos.UpdateUser(user);
            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return;
        }
    }
}