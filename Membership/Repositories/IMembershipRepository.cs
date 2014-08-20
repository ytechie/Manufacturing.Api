using Manufacturing.Api.Membership.Models;
using System;
using System.Collections.Generic;

namespace Manufacturing.Api.Membership.Repositories
{
    public interface IMembershipRepository
    {
        UserDetail CreateUser(UserToCreate user);
        IEnumerable<Role> GetAllRoles();
        IEnumerable<UserSummary> GetAllUsers();
        UserDetail GetUser(string userId);
        UserDetail UpdateUser(UserToUpdate user);
    }
}
