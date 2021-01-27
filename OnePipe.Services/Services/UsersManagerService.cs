using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OnePipe.Core;
using OnePipe.Core.DTO;
using OnePipe.Core.Entities;
using OnePipe.Core.Enum;
using OnePipe.Core.Services;
using Scrutor.AspNetCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnePipe.Service.Services
{
    public class UsersManagerService : IUsersManagerService, IScopedLifetime
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersManagerService(UserManager<Users> userManager, RoleManager<UserRole> roleManager, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseMessageHandler> AddNewUser(Users user)
        {
            var response = new ResponseMessageHandler();
            var userExist = (await _userManager.FindByEmailAsync(user.Email)) != null;

            if (!userExist)
            {
                //add a password generator here to repace the default one
                string password = "1234567";
                user.Roles = new List<string> { user.EmployeeType.ToString() };
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    //add role and permission

                    if (!await _roleManager.RoleExistsAsync(user.EmployeeType.ToString()))
                    {
                        await _userManager.AddToRoleAsync(user, user.EmployeeManager.ToString());
                    }
                    else
                    {
                        var role = new UserRole();
                        role.Name = user.EmployeeManager.ToString();
                        await _roleManager.CreateAsync(role);
                        await _userManager.AddToRoleAsync(user, user.EmployeeManager.ToString());
                    }
                }

                response.status = result.Succeeded ? "success" : "failed";
                return response;
            }
            {
                response.status = "failed";
                response.ErrorMessages.Add("User with the same email exist");
            }

            return response;
        }

        public async Task<string> Login(string email, string password)
        {
            string tokenString = string.Empty;
            var user = await AuthenticateUsers(email, password);

            if (user != null)
            {
                var roles = await UserRoles(user);
                var userRoles = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray();
                var userClaims = await GetUserClaimAsync(user);
                var roleClaims = await GetRoleClaimsAsync(roles).ConfigureAwait(false);
                tokenString = GenerateJSONWebToken(user, userRoles, userClaims, roleClaims);                
            }
            return tokenString;
        }

        private async Task<Users> AuthenticateUsers(string email, string password)
        {
            var user = await _userManager.FindByNameAsync(email);

            if (user == null)
            {
                throw new Exception("Email not confirmed");
            }
            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result)
            {
                throw new Exception("Invalid Credentials");
            }
            return user;
        }

        private string GenerateJSONWebToken(Users userInfo, Claim[] userRoles, IList<Claim> userClaims, IList<Claim> roleClaims)    
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Vaibhajddjdkjdmojdijdiue738439eujd98iejkekddkdkiue83eu8ueurjedksi8e33y63738393474663yehejdkdjhvBhapkar"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim("Id", userInfo.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }.Union(userClaims).Union(roleClaims).Union(userRoles);

            var token = new JwtSecurityToken(
                issuer: "onepipe.com",
                audience: "APIClient",
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<IList<string>> UserRoles(Users user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        private async Task<IList<Claim>> GetUserClaimAsync(Users user)
        {
            return await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
        }

        public async Task<IList<Claim>> GetRoleClaimsAsync(IList<string> roleName)
        {
            List<Claim> claims = new List<Claim>();
            foreach (var rawData in roleName)
            {
                var data = await _roleManager.FindByNameAsync(rawData);
                if (data != null)
                {
                    var result = await _roleManager.GetClaimsAsync(data).ConfigureAwait(false);
                    if (result.Any())
                    {
                        claims.AddRange(result);
                    }
                }
            }
            return claims;
        }

        public async Task<List<Users>> GetUsers()
        {
            //Login User
            var userId = _httpContextAccessor.HttpContext.User.FindFirst("Id").Value;
            var user = await _unitOfWork.User.GetAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            var employees = new List<Users>();

            foreach(string role in roles)
            {
                EmployeeType employeeType;
                Enum.TryParse(role, out employeeType);

                switch (employeeType)
                {
                    case EmployeeType.HR:
                        employees = await GetHRUsers(user, employees);
                        break;
                    case EmployeeType.Manager:
                        employees = await GetManagerUsers(user, employees);
                        break;
                    default:
                        break;
                }                    
            }
            return employees;
         }

        private async Task<List<Users>> GetManagerUsers(Users user, List<Users> employees)
        {
            var employeeId = user.EmployeeManager.Select(x => x.EmployeeId).ToList();
            var managerUsers = await _unitOfWork.User.GetEmpoyeeForManager(employeeId);

            //work on Exclude duplicate user adding in the loop

            employees.AddRange(managerUsers);
            return employees;
        }

        private async Task<List<Users>> GetHRUsers(Users user, List<Users> employees)
        {
            //work on the predicate to reduce filtering to the database
            var hrUser = await _unitOfWork.User.GetEmpoyeeForHR();

            //work on Exclude duplicate user adding in the loop
            employees.AddRange(hrUser);
            return employees;
        }

        public Task<ResponseMessageHandler> UpdateUser(string userid, Users user)
        {
            throw new NotImplementedException();
        }

        public Task<Users> GetUsers(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddEmployeeToManager(string userId, string managerId)
        {
            bool result = false;
            var manager = _userManager.Users.Where(x => x.Id == managerId).FirstOrDefault();

            if(manager != null)
            {
                //assume staff is not null also
                //assume the user not attached to the user before
                var emp = new EmployeeManager
                {
                    EmployeeId = userId,
                    ManagerId = managerId
                };

                manager.EmployeeManager.Add(emp);
                result = true;
            }

            return result;
        }
    }
}
                                         