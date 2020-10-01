using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CKSource.CKFinder.Connector.Core;
using CKSource.CKFinder.Connector.Core.Authentication;

namespace WebSite.Infrastructure.Security
{
    public class CustomCKFinderAuthenticator : IAuthenticator
    {
        public Task<IUser> AuthenticateAsync(ICommandRequest commandRequest, CancellationToken cancellationToken)
        {
            // var claimsPrincipal = commandRequest.Principal as ClaimsPrincipal;
            // var roles = claimsPrincipal?.Claims?.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToArray();
            /*
             * Enable CKFinder only for authenticated users.
             */
            //var isAuthenticated = claimsPrincipal.Identity.IsAuthenticated;
            //var user = new User(isAuthenticated, roles);
            var claimsPrincipal = commandRequest.Principal as ClaimsPrincipal;
            var roles = claimsPrincipal?.Claims?.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToArray();
            bool canuse = false;

            if (roles.Contains("admin"))
                canuse = true;

            var user = new User(canuse, roles);
            return Task.FromResult((IUser)user);
        }
    }
}