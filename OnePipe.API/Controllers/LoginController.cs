using Microsoft.AspNetCore.Mvc;
using OnePipe.Core.DTO;
using OnePipe.Core.Services;
using System.Threading.Tasks;

namespace OnePipe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IUsersManagerService _userManagerService;

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
    }
}
