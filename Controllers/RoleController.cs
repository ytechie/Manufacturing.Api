using Manufacturing.Api.Membership.Attributes;
using Manufacturing.Api.Membership.Models;
using Manufacturing.Api.Membership.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Manufacturing.Api.Controllers
{
    [AuthorizeMembershipInfoAccessAttribute(Roles = new[] { "Administrators" })]
    public class RoleController : ApiController
    {
        private IMembershipRepository _repos;

        public RoleController(IMembershipRepository repos)
        {
            _repos = repos;
        }

        // GET api/role
        public IEnumerable<Role> Get()
        {
            return _repos.GetAllRoles();
        }
    }
}
