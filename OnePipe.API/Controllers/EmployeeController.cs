using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnePipe.Core.DTO;
using OnePipe.Core.Entities;
using OnePipe.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [Authorize("UserShouldAccessRecord")]
        [HttpGet]
        [Route("GetEmployees")]
        public async Task<IList<Users>> GetEmployees()
        {
            var result = await _userManagerService.GetUsers();
            return result;
        }

        [Authorize("UserShouldUpdateRecord")]
        [HttpPut]
        [Route("UpdateEmployee/{userId}")]
        public async Task<ResponseMessageHandler> UpdateEmployees(string userId, Users user)
        {
            var result = await _userManagerService.UpdateUser(userId, user);
            return result;
        }

        [Authorize("UserShouldAccessRecord")]
        [HttpGet]
        [Route("GetEmployee/{userId}")]
        public async Task<Users> GetEmployee(string userId)
        {
            var result = await _userManagerService.GetUsers(userId);
            return result;
        }

    }
}
