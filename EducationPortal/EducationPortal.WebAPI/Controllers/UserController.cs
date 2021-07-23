using EducationPortal.Core;
using EducationPortal.WebAPI;
using EducationPortal.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EducationPortal.WebAPI.Filters;

namespace EducationPortal.MVC.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : Controller
    {
        private IUserService _userService;
        private ITokenService _tokenService;

        public UserController( IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Route("GetUserData")]
        [ServiceFilter(typeof(AuthentificationFilter))]
        public async Task<IActionResult> GetUserData(string token)
        {
            if (await _tokenService.VerifyToken(token))
            {
                var user = await _userService.GetPersonalDataAsync(_tokenService.GetUserIdByToken(token));
                user.Password = "";
                return new ObjectResult(user);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetAvailableCourse")]
        [ServiceFilter(typeof(AuthentificationFilter))]
        public async Task<IActionResult> GetAvailableCourses(string token ,int pageNumber = 1, int pageSize = 10)
        {
            if (await _tokenService.VerifyToken(token))
            {
                var courseList = await _userService.GetAvailableCoursesAsync(_tokenService.GetUserIdByToken(token), pageNumber, pageSize);
                return new ObjectResult(courseList);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("ChangePersonalData")]
        [ServiceFilter(typeof(AuthentificationFilter))]
        public async Task<IActionResult> ChangePersonalData(string token, UserViewModel user)
        {
            if (await _tokenService.VerifyToken(token))
            {
                if (ModelState.IsValid)
                {
                    var result = false;
                    var id = _tokenService.GetUserIdByToken(token);
                    var previousUserData = await _userService.GetPersonalDataAsync(id);
                    previousUserData.Email = user.Email;
                    previousUserData.Phone = user.Phone;
                    previousUserData.Name = user.Name;
                    result = await _userService.ChangePersonalDataAsync(previousUserData);
                    if (result)
                    {
                        return new OkResult();
                    }
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("LearnMaterial")]
        [ServiceFilter(typeof(AuthentificationFilter))]
        public async Task<IActionResult> LearnMaterial(string token, int materialId, int courseId)
        {
            if (await _tokenService.VerifyToken(token))
            {
                var result = false;
                result = await _userService.LearnMaterialAsync(_tokenService.GetUserIdByToken(token), materialId);
                if (result)
                {
                    return new OkResult();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("StartCourse")]
        [ServiceFilter(typeof(AuthentificationFilter))]
        public async Task<IActionResult> StartCourse(string token, int courseId)
        {
            if (await _tokenService.VerifyToken(token))
            {
                var result = await _userService.StartLearnCourseAsync(_tokenService.GetUserIdByToken(token), courseId);
                if (result)
                {
                    return new OkResult();
                }
            }
            return BadRequest();
        }
    }
}
