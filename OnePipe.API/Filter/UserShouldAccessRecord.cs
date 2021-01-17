using Microsoft.AspNetCore.Authorization;
using OnePipe.Core.Enum;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnePipe.API.Filter
{
    public class UserShouldAccessRecord : IAuthorizationRequirement
    {
    }

    public class UserShouldAccessRecordRequirementHandler : AuthorizationHandler<UserShouldAccessRecord>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserShouldAccessRecord requirement)
        {
            if (!context.User.HasClaim(x => x.Type == ClaimTypes.Role))
                return Task.CompletedTask;

            // claim exists - retrieve the value
            var claim = context.User
                .Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            var role = claim.Value;

            //This need to change to Role Name with the claim attached to the role
            if (role == EmployeeType.Administrative.ToString() || role == EmployeeType.HR.ToString() || role == EmployeeType.Manager.ToString())
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
