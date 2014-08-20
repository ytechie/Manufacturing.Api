using Manufacturing.Api.Membership.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WAAD = Microsoft.WindowsAzure.ActiveDirectory;

namespace Manufacturing.Api.Membership.Extensions
{
    public static class MembershipExtensions
    {
        public static UserSummary ToUserSummary(this WAAD.User user)
        {
            var detail = new UserSummary
            {
                Id = user.objectId,
                Name = user.displayName,
                UserName = user.userPrincipalName,
            };
            return detail;
        }

        public static UserDetail ToUserDetail(this WAAD.User user, IEnumerable<WAAD.Group> groups)
        {
            var detail = new UserDetail
                {
                    Id = user.objectId,
                    Name = user.displayName,
                    UserName = user.userPrincipalName,
                    Roles = groups != null ? groups.Select(x => x.ToRole()) : Enumerable.Empty<Role>()
                };
            return detail;
        }

        public static Role ToRole(this WAAD.Group group)
        {
            var role = new Role
            {
                Id = group.objectId,
                Name = group.displayName,
                Description = group.description,
            };
            return role;
        }

        public static WAAD.User ToWaadUser(this UserToCreate user, string domain)
        {
            // Setting required WAAD fields here based on what we were given
            var waadUser = new WAAD.User
                {
                    accountEnabled = true,
                    displayName = user.Name,
                    mailNickname = user.MailNickName,
                    passwordProfile = new WAAD.PasswordProfile { forceChangePasswordNextLogin = false, password = user.Password },
                    userPrincipalName = String.Format("{0}@{1}",user.MailNickName,"chrisdir.onmicrosoft.com")
                };
            return waadUser;
        }
    }
}