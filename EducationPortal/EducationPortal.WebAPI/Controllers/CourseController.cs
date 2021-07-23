using EducationPortal.Core;
using EducationPortal.MVC.Models;
using EducationPortal.WebAPI;
using EducationPortal.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EducationPortal.MVC.Controllers
{
    [ApiController]
    [Route("Course")]
    public class CourseController : Controller
    {
        private IUserService _userService;
        private ICourseService _courseService;
        private ITokenService _tokenService;

        public CourseController(ICourseService courseService, ITokenService tokenService, IUserService userService)
        {
            _userService = userService;
            _courseService = courseService;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Route("GetCourse")]
        [ServiceFilter(typeof(AuthentificationFilter))]
        public async Task<IActionResult> GetCourse(string token, int courseId)
        {
            var course = await _courseService.GetCourseAsync(courseId);
            return new ObjectResult(course);
        }

        [HttpPost]
        [Route("GetCourseCreatedByCurrentUser")]
        [ServiceFilter(typeof(AuthentificationFilter))]
        public async Task<IActionResult> GetCoursesCreatedByCurrentUserButNotPublished(string token, int page = 1, int pageSize = 10)
        {
            var coursePagedList = await _userService.GetCreatedNotPublishedCoursesAsync(
                 _tokenService.GetUserIdByToken(token), page, pageSize);
            return new ObjectResult(coursePagedList);
        }

        [HttpPost]
        [Route("CreateCourse")]
        [ServiceFilter(typeof(AuthentificationFilter))]
        public async Task<IActionResult> CreateCourse(string token, CourseViewModel course)
        {
            if (ModelState.IsValid)
            {
                var result = false;
                result = await _courseService.AddCourseAsync(_tokenService.GetUserIdByToken(token),
                    new Course
                    {
                        Name = course.Name,
                        Description = course.Description,
                        IsPublished = false
                    });
                if (result)
                {
                    return new OkResult();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("AddAdditions")]
        [ServiceFilter(typeof(AuthentificationFilter))]
        public async Task<IActionResult> AddAdditions(string token, AdditionViewModel add, int courseId)
        {
            if (ModelState.IsValid)
            {
                bool result = false;
                switch (add.AdditionType)
                {
                    case AdditionTypeEnum.GivenSkill:
                        result = await _courseService.AddSkillsToCourseAsync(
                            _tokenService.GetUserIdByToken(token), add.AdditionId,
                            courseId);
                        break;
                    case AdditionTypeEnum.RequirenmentSkill:
                        result = await _courseService.AddRequirenmentToCourseAsync(
                            _tokenService.GetUserIdByToken(token),
                            new RequirenmentSkill { CourseId = courseId, Level = add.Level, SkillId = add.AdditionId },
                            courseId);
                        break;
                    case AdditionTypeEnum.Material:
                        result = await _courseService.AddMaterialToCourseAsync(
                            _tokenService.GetUserIdByToken(token), add.AdditionId,
                            courseId);
                        break;
                }
                if (result)
                {
                    return new OkResult();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("PublishCourse")]
        [ServiceFilter(typeof(AuthentificationFilter))]
        public async Task<IActionResult> PublishCourse(string token, int courseId)
        {
            var result = await _courseService.PublishCourseAsync(_tokenService.GetUserIdByToken(token), courseId);
            if (result)
            {
                return new OkResult();
            }
            return BadRequest();
        }
    }
}
