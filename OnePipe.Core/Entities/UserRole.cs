using Microsoft.AspNetCore.Identity.MongoDB;

namespace OnePipe.Core.Entities
{
    public class UserRole : IdentityRole
    {
        public UserRole()
        {
        }

        public UserRole(string roleName) : base(roleName)
        {
        }
    }
}
