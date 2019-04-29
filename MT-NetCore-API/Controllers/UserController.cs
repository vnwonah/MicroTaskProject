using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MT_NetCore_API.Helpers;
using MT_NetCore_API.Interfaces;
using MT_NetCore_API.Models.AuthModels;
using MT_NetCore_API.Models.ResponseModels;
using MT_NetCore_Common.Interfaces;
using MT_NetCore_Data.IdentityDB;
using Newtonsoft.Json;


namespace MT_NetCore_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : BaseController
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ITenantRepository _tenantRepository;

        public UserController(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtIssuerOptions> jwtOptions,
            IJwtFactory jwtFactory,
            IRequestContext requestContext,
            ITenantRepository tenantRepository) : base(requestContext)
        {
            
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _tenantRepository = tenantRepository;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) 
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorDescription = "Your Email or Password is Incorrect"
                });
            }
            return Ok(new RegisterResponse
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,

            });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var identity = await GetClaimsIdentity(model.Email, model.Password);

                if (identity == null)
                {
                    return BadRequest(new ErrorResponse
                    {
                        ErrorDescription = "Your Email or Password is Incorrect"
                    });
                }
                var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, model.Email, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

                return Ok(new LoginResponse
                {
                    Token = jwt
                });
            }

            return BadRequest(new ErrorResponse
            {
                ErrorDescription = "Your Email or Password is Incorrect"
            });
           
        }

        [HttpGet(nameof(GetUserForTeam))]
        public async Task<IActionResult> GetUserForTeam(string email)
        {
            var user = await _tenantRepository.GetUserByEmailAsync(email, TenantId);
            if(user != null)
                return Ok(user);
            return BadRequest(new ErrorResponse
            {
                ErrorDescription = "User does not have access to team or Team does not Exist"
            });
        }

        [HttpGet(nameof(GetUserProjects))]
        public async Task<IActionResult> GetUserProjects(string email)
        {
            var projects = await _tenantRepository.GetUserProjects(email, TenantId);
            if (projects != null)
                return Ok(projects);
            return BadRequest(new ErrorResponse
            {
                ErrorDescription = "User is not assigned a project in this Team"
            });
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByEmailAsync(email);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(email, userToVerify.Id));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

    }
}
