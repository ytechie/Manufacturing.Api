using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing.Api.Membership.Models
{
    public class UserSummary
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
    }

    public class UserDetail
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }

    public class UserToCreate
    {
        public string Name { get; set; }
        public string MailNickName { get; set; } // e.g., MailNickName@domain
        public string Password { get; set; }
        public IEnumerable<string> DesiredRoleIds { get; set; }
    }

    public class UserToUpdate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> DesiredRoleIds { get; set; }
    }
}