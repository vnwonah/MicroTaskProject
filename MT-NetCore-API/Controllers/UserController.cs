using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MT_NetCore_API.Helpers;
using MT_NetCore_API.Interfaces;
using MT_NetCore_API.Models.Auth;

namespace MT_NetCore_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserHelper _userHelper;

        public UserController(
            IUserService userService,
            UserHelper userHelper)
        {
            _userService = userService;
            _userHelper = userHelper;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody]LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (_userService.Login(model))
                {
                    _userHelper.GenerateToken(model.Email);
                }
            }

            return BadRequest();
           
        }

    }
}
