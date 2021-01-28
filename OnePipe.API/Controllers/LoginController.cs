using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using OnePipe.Core.DTO;
using OnePipe.Core.Entities;
using OnePipe.Core.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using OnePipe.API.Models;
using OnePipe.API.Services;
using OnePipe.Core;
using IUsersManagerService = OnePipe.API.Services.IUsersManagerService;

namespace OnePipe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IUsersManagerService _userManagerService;
        private UserManager<Users> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(UserManager<Users> userManager, RoleManager<UserRole> roleManager, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _userManagerService = new UsersManagerService(userManager, roleManager,unitOfWork, httpContextAccessor);
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(UserLogin login)
        {
            IActionResult response = Unauthorized();
            var token = await _userManagerService.Login(login.UserName, login.Password);
            response = Ok(new
            {
                token = token,
                email = login.UserName
            });

            return response;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(NewUser user)
        {
            IActionResult response = Unauthorized();
            var token = await _userManagerService.Login(user.Email, user.Password);
            
            response = Ok(new
            {
                 token
            });

            return response;
        }


        [HttpPost("add_employee")]
        public async Task<IActionResult> RegisterAsync(NewUser user)
        {
            IActionResult response = Unauthorized();
            var token = await _userManagerService.AddNewUser(new Users
            {
                Email = user.Email,
                PasswordHash = user.Password,
                UserName = user.Email,
                UserType = user.UserType,
                SalaryHistory = user.SalaryHistories,
                ManagedById = user.ManagedById,
                Claims = PermissionHelper.GetEmployeePermissions(user.UserType).Select(c => new IdentityUserClaim
                {
                    Type = c.Code,
                    Value = c.Name
                }).ToList()
            });

            response = Ok(new
            {
                token
            });

            return response;
        }



        [HttpGet("get_employee/{email}")]
        public async Task<IActionResult> GetEmployee(string email)
        {
            var user = await _userManagerService.GetUsers(email);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(new NewUser
            {
                UserType = user.UserType,
                Email = user.Email,
                ManagedById = user.ManagedById,
                SalaryHistories = user.SalaryHistory,
            });
        }

        [HttpGet("get_employees")]
        [Authorize("AccessAllEmployee")]
        public async Task<IActionResult> GetEmployeesTask()
        {

            var users = new List<Users>();

            if (User.IsInRole("HR"))
            {
               // users = await _userManagerService.GeHRtUsers();
            }
            else
            {
                users = await _userManagerService.GetUsers();
            }

            if (users == null)
            {
                return NotFound();
            }

            var response = new ResponseMessageHandler();
            response.data = users.Select(c => new
            {
                c.Email,
                c.ManagedById,
                c.UserType
            });
            return Ok(response);
        }

        [HttpGet("get_my_employees/{id}")]
        [Authorize("AccessManagerEmployee")]
        public async Task<IActionResult> GetMyEmployees(string id)
        {
            var users = await _userManagerService.GetMyUsers(id);

            if (users == null)
            {
                return NotFound();
            }

            var response = new ResponseMessageHandler();
            response.data = users.Select(c => new
            {
                c.Email,
                c.ManagedById,
                c.UserType
            });
            return Ok(response);
        }

        [HttpPost]
        [Route("AddEmployee")]
        public async Task<ResponseMessageHandler> AddEmployees(Users user)
        {
            var result = await _userManagerService.AddNewUser(user);
            return result;
        }

        [HttpPost]
        [Route("AddEmployeeToManager")]
        public async Task<bool> AddEmployeeToManager(string userId, string managerId)
        {
            var result = await _userManagerService.AddEmployeeToManager(userId, managerId);
            return result;
        }
    }
}
