using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using EducationPortal.MVC.Models;
using EducationPortal.Core;
using System.Security.Cryptography;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace EducationPortal.MVC.Controllers
{
    public class AuthController : Controller
    {
        private IAuthService _authService;
        private IUserService _userService;
        private IValidator<LoginViewModel> _loginValidator;
        private IValidator<RegistrationViewModel> _registrationValidator;

        public AuthController(IAuthService authService, IUserService userService,
            IValidator<LoginViewModel> loginValidator, IValidator<RegistrationViewModel> registrationValidator)
        {
            _authService = authService;
            _userService = userService;
            _loginValidator = loginValidator;
            _registrationValidator = registrationValidator;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View("Registration");
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var validate = _loginValidator.Validate(model);
            if (validate.IsValid)
            {
                int id = _authService.LoginAsync(model.Login, model.Password).GetAwaiter().GetResult();
                if (id > 0)
                {
                    var options = new CookieOptions();
                    options.Expires = DateTime.Now.AddDays(1);
                    options.Secure = true;
                    this.HttpContext.Response.Cookies.Append("UserId", id.ToString(),options);
                    this.HttpContext.Response.Cookies.Append("UserLogin", _userService.GetUserLoginAsync(id).GetAwaiter().GetResult(), options);
                    this.HttpContext.Response.Cookies.Append("UserToken", CreateToken(id), options);
                    this.HttpContext.Response.Redirect("/Home/Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Incorrect login+password combination";
                }
            }
            ViewBag.Errors = validate.Errors;
            ViewBag.Model = model;
            return View("Login");
        }
        [HttpPost]
        public IActionResult Registration(RegistrationViewModel model)
        {
            var validate = _registrationValidator.Validate(model);
            if (validate.IsValid)
            {
                var user = new User
                {
                    Login = model.Login,
                    Password = model.Password,
                    Email = model.Email,
                    Name = model.Name,
                    Phone = model.Phone
                };
                if (_authService.RegisterAsync(user).GetAwaiter().GetResult())
                {
                    ViewBag.SuccessMessage = "You registrated successfully";
                }
                else
                {
                    ViewBag.ErrorMessage = "Registration failed";
                }
            }
            else
            {
                ViewBag.Errors = validate.Errors;
            }
            ViewBag.Model = model;
            return View("Registration");
        }
        public void Logout()
        {
            this.Response.Cookies.Delete("UserId");
            this.Response.Cookies.Delete("UserLogin");
            this.Response.Cookies.Delete("UserToken");
            this.Response.Redirect("/Home/Index");
        }
        
        public bool VerifyToken()
        {
            int id;
            if (int.TryParse(this.HttpContext.Request.Cookies["UserId"], out id))
            {
                var token = CreateToken(id);
                return this.HttpContext.Request.Cookies["UserId"] == token;
            }
            return false;
        }

        private string CreateToken(int id)
        {
            var user = _userService.GetPersonalDataAsync(id).GetAwaiter().GetResult();
            string rawData = $"Token {user.Id} {user.Login} {user.Password} End Token";
            using (var hashAlgorithm = SHA512.Create())
            {
                byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                var sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }
}
