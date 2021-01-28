using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnePipe.Core.DTO;
using OnePipe.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using IUsersManagerService = OnePipe.API.Services.IUsersManagerService;

namespace OnePipe.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IUsersManagerService _userManagerService;

        public EmployeeController(IUsersManagerService usersManagerService)
        {
            _userManagerService = usersManagerService;
        }

        [Authorize("AccessAllEmployee", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("all")]
        public async Task<IList<Users>> GetEmployees()
        {
            var result = await _userManagerService.GetUsers();
            return result;
        }

        [HttpPut]
        [Route("UpdateEmployee/{userId}")]
        [Authorize("AccessManagerEmployee", AuthenticationSchemes = "Bearer")]
        public async Task<ResponseMessageHandler> UpdateEmployees(string userId, Users user)
        {
            var result = await _userManagerService.UpdateUser(userId, user);
            return result;
        }

        [HttpGet]
        [Route("GetEmployee/{userId}")]
        [Authorize("AccessManagerEmployee", AuthenticationSchemes = "Bearer")]
        public async Task<Users> GetEmployee(string userId)
        {
            var result = await _userManagerService.GetUsers(userId);
            return result;
        }

    }
}
