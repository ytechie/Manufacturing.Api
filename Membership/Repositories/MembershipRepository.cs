using WAAD = Microsoft.WindowsAzure.ActiveDirectory;
using Models = Manufacturing.Api.Membership.Models;
using Manufacturing.Api.Membership.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Services.Client;
using Microsoft.WindowsAzure.ActiveDirectory;

namespace Manufacturing.Api.Membership.Repositories
{
    // Source of membership-related information
    public class MembershipRepository : Manufacturing.Api.Membership.Repositories.IMembershipRepository
    {
        private AuthenticationConfiguration _authConfig;

        public MembershipRepository(AuthenticationConfiguration config)
        {            
            // if multiple domains in place for WAAD tenant this needs to change
            _authConfig = config;
        }

        // No paging support for now
        public IEnumerable<Models.Role> GetAllRoles()
        {
            // We're actually using the "Groups" in WAAD as the roles at this point
            WAAD.DirectoryDataService graphService = DirectoryDataServiceFactory.GetInstance(_authConfig);
            QueryOperationResponse<WAAD.Group> response = graphService.groups.Execute() as QueryOperationResponse<WAAD.Group>;

            List<WAAD.Group> groupList = response.ToList();
            while (response.GetContinuation() != null)
            {
                // Grab the next page of results
                response = graphService.Execute<WAAD.Group>(response.GetContinuation().NextLinkUri) as QueryOperationResponse<WAAD.Group>;
                groupList.AddRange(response.ToList());
            }

            return groupList.Select(x => x.ToRole());
        }

        // No paging support for now
        public IEnumerable<Models.UserSummary> GetAllUsers()
        {
            WAAD.DirectoryDataService graphService = DirectoryDataServiceFactory.GetInstance(_authConfig);
            QueryOperationResponse<WAAD.User> response = graphService.users.Execute() as QueryOperationResponse<WAAD.User>;

            List<WAAD.User> userList = response.ToList();
            while (response.GetContinuation() != null)
            {
                // Grab the next page of results
                response = graphService.Execute<WAAD.User>(response.GetContinuation().NextLinkUri) as QueryOperationResponse<WAAD.User>;
                userList.AddRange(response.ToList());
            }

            return userList.Select(x => x.ToUserSummary());
        }

        public Models.UserDetail GetUser(string userId)
        {
            WAAD.DirectoryDataService graphService = DirectoryDataServiceFactory.GetInstance(_authConfig);
            WAAD.User user = graphService.users.Where(x => x.objectId == userId).SingleOrDefault();
            if (user == null)
            {
                return null;
            }

            // Grab roles for the user
            graphService.LoadProperty(user, "memberOf");
            var currentGroups = user.memberOf.OfType<WAAD.Group>();

            return user.ToUserDetail(currentGroups);
        }

        public Models.UserDetail CreateUser(Models.UserToCreate user)
        {
            WAAD.DirectoryDataService graphService = DirectoryDataServiceFactory.GetInstance(_authConfig);
            // Note here that if tenant has multiple domains, the domain to use would need
            // to be specified on the request to us instead
            WAAD.User waadUser = user.ToWaadUser(_authConfig.DirectoryDomain);
            graphService.AddTousers(waadUser);

            var allGroups = graphService.directoryObjects.OfType<WAAD.Group>().ToList();
            foreach (var groupId in user.DesiredRoleIds)
            {
                graphService.AddLink(allGroups.First(x => x.objectId == groupId), "members", waadUser);
            }
            
            graphService.SaveChanges();
                                    
            return waadUser.ToUserDetail(Enumerable.Empty<WAAD.Group>());
        }

        public Models.UserDetail UpdateUser(Models.UserToUpdate user)
        {
            WAAD.DirectoryDataService graphService = DirectoryDataServiceFactory.GetInstance(_authConfig);
            WAAD.User waadUser = graphService.users.Where(x => x.objectId == user.Id).SingleOrDefault();
            if (waadUser == null)
            {
                return null;
            }
            
            // Load user's current groups so we know what we have to do to have things reflect desired state
            graphService.LoadProperty(waadUser, "memberOf");
            var userCurrentGroupIds = waadUser.memberOf.OfType<WAAD.Group>().Select(x => x.objectId).ToList();

            // Load all groups
            var allGroups = graphService.directoryObjects.OfType<WAAD.Group>().ToList();

            // See which groups user has now but no longer wants
            foreach (var groupIdToRemove in userCurrentGroupIds.Except(user.DesiredRoleIds))
            {
                var groupToRemoveLinkFrom = allGroups.FirstOrDefault(x => x.objectId == groupIdToRemove);
                if (groupToRemoveLinkFrom != null)
                {
                    graphService.DeleteLink(groupToRemoveLinkFrom, "members", waadUser);    
                }                
            }
            // See which new groups user wants
            foreach (var groupIdToAdd in user.DesiredRoleIds.Except(userCurrentGroupIds))
            {
                var groupToAddLinkTo = allGroups.FirstOrDefault(x => x.objectId == groupIdToAdd);
                if (groupToAddLinkTo != null)
                {
                    graphService.AddLink(groupToAddLinkTo, "members", waadUser);
                }                                
            }

            waadUser.displayName = user.Name;
            graphService.UpdateObject(waadUser);

            graphService.SaveChanges();

            // At this point, does waadUser reflect updated links in its memberOf prop that we previously loaded?
            return waadUser.ToUserDetail(allGroups.Where(x => user.DesiredRoleIds.Contains(x.objectId)));
        }

    }
}