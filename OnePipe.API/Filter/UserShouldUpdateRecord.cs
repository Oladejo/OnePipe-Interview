using Microsoft.AspNetCore.Authorization;
using OnePipe.Core.Enum;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnePipe.API.Filter
{
    public class UserShouldUpdateRecord : IAuthorizationRequirement
    {
    }

    public class UserShouldUpdateRecordRequirementHandler : AuthorizationHandler<UserShouldUpdateRecord>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserShouldUpdateRecord requirement)
        {
            if (!context.User.HasClaim(x => x.Type == ClaimTypes.Role))
                return Task.CompletedTask;

            // claim exists - retrieve the value
            var claim = context.User
                .Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            var role = claim.Value;

            if (role == EmployeeType.Administrative.ToString() || role == EmployeeType.Manager.ToString())
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
