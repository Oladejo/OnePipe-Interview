using Microsoft.AspNetCore.Mvc;
using OnePipe.Core.DTO;
using OnePipe.Core.Entities;
using OnePipe.Core.Services;
using System.Threading.Tasks;

namespace OnePipe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IUsersManagerService _userManagerService;

        public LoginController(IUsersManagerService usersManagerService)
        {
            _userManagerService = usersManagerService;
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
