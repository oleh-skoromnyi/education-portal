using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EducationPortal.MVC.Models;
using EducationPortal.Core;
using EducationPortal.WebAPI;

namespace EducationPortal.MVC.Controllers
{
    [ApiController]
    [Route("Auth")]
    public class AuthController : Controller
    {
        private IAuthService _authService;
        private IUserService _userService;
        private ITokenService _tokenService;

        public AuthController(IAuthService authService, IUserService userService,ITokenService tokenService)
        {
            _authService = authService;
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                int id = await _authService.LoginAsync(model.Login, model.Password);
                if (id > 0)
                {
                    var token = await _tokenService.CreateToken(id);
                    return new ObjectResult(token);
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Login = model.Login,
                    Password = model.Password,
                    Email = model.Email,
                    Name = model.Name,
                    Phone = model.Phone
                };
                if (await _authService.RegisterAsync(user))
                {
                    var id = await _authService.LoginAsync(user.Login, user.Password);
                    var token = await _tokenService.CreateToken(id);
                    return new ObjectResult(token);
                }
            }
            return BadRequest();
        }

    }
}
