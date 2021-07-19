using EducationPortal.Core;
using EducationPortal.Core.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPortal.MVC.Controllers
{
    public class UserController : Controller
    {
        private IUserService _userService;
        private ICourseService _courseService;
        private ISkillService _skillService;
        private IMaterialService _materialService;
        private IValidator<User> _userValidator;
        private IConfiguration _config;

        public UserController(ICourseService courseService, IValidator<User> userValidator,
             IConfiguration config, ISkillService skillService,
             IMaterialService materialService, IUserService userService)
        {
            _userService = userService;
            _courseService = courseService;
            _skillService = skillService;
            _materialService = materialService;
            _userValidator = userValidator;
            _config = config;
        }

        public IActionResult Index()
        {
            ResponsePreparing();
            return View();
        }

        public IActionResult GoToCourse(int courseId)
        {
            MaterialsResponsePreparing(courseId);
            return View("GoToCourse");
        }


        public IActionResult ChangePersonalData(User user)
        {
            var validate = _userValidator.Validate(user);
            if (validate.IsValid)
            {
                var result = false;
                var temp = user;
                temp.Id = int.Parse(this.Request.Cookies["UserId"]);
                result = _userService.ChangePersonalDataAsync(temp).GetAwaiter().GetResult();
                if (result)
                {
                    ViewBag.SuccessMessage = "Personal data changed successfully";
                }
                else
                {
                    ViewBag.ErrorMessage = "Personal data changing failed";
                }
            }
            else
            {
                ViewBag.Errors = validate.Errors;
            }
            ResponsePreparing();
            return View("Index");
        }

        public IActionResult LearnMaterial(int materialId, int courseId)
        {
            bool result = false;
            result = _userService.LearnMaterialAsync(int.Parse(this.Request.Cookies["UserId"]), materialId).GetAwaiter().GetResult();
            if (result)
            {
                ViewBag.SuccessMessage = "Material learn successfully";
            }
            else
            {
                ViewBag.ErrorMessage = "Material learning failed";
            }
            MaterialsResponsePreparing(courseId);
            return View("GoToCourse");
        }
        public IActionResult StartCourse(int courseId)
        {
            var result = _userService.StartLearnCourseAsync(int.Parse(this.Request.Cookies["UserId"]), courseId).GetAwaiter().GetResult();
            if (result)
            {
                ViewBag.SuccessMessage = "Course published successfully";
            }
            else
            {
                ViewBag.ErrorMessage = "Course publish failed";
            }
            ResponsePreparing();
            return View("Index");
        }

        [HttpPost]
        public IActionResult ChangePage(int userCoursePage = 1, int userSkillPage = 1, int availableCoursePage = 1)
        {
            ResponsePreparing(userCoursePage, userSkillPage, availableCoursePage);
            return View("Index");
        }

        [HttpPost]
        public IActionResult ChangePageInMaterials(int page)
        {
            MaterialsResponsePreparing(page);
            return View("Course");
        }
        private void ResponsePreparing(int userCoursePage = 1, int userSkillPage = 1, int availableCoursePage = 1)
        {
            var userCoursesPagedList = _userService.GetCoursesAsync(int.Parse(this.Request.Cookies["UserId"]), userCoursePage, _config.GetValue<int>("PageSize")).GetAwaiter().GetResult();
            var userSkillsPagedList = _userService.GetSkillsAsync(int.Parse(this.Request.Cookies["UserId"]), userSkillPage, _config.GetValue<int>("PageSize")).GetAwaiter().GetResult();
            var availableCoursesPagedList = _userService.GetAvailableCoursesAsync(int.Parse(this.Request.Cookies["UserId"]), availableCoursePage, _config.GetValue<int>("PageSize")).GetAwaiter().GetResult();

            var user = _userService.GetPersonalDataAsync(int.Parse(this.Request.Cookies["UserId"])).GetAwaiter().GetResult();

            ViewBag.userName = user.Name;
            ViewBag.userEmail = user.Email;
            ViewBag.userPhone = user.Phone;

            ViewBag.userCoursesPageCount = userCoursesPagedList.PageCount;
            ViewBag.userCoursesItems = userCoursesPagedList.Items;
            ViewBag.userCoursesPageNumber = userCoursesPagedList.PageNumber;

            ViewBag.userSkillsPageCount = userSkillsPagedList.PageCount;
            ViewBag.userSkillsItems = userSkillsPagedList.Items;
            ViewBag.userSkillsPageNumber = userSkillsPagedList.PageNumber;

            ViewBag.availableCoursesPageCount = availableCoursesPagedList.PageCount;
            ViewBag.availableCoursesItems = availableCoursesPagedList.Items;
            ViewBag.availableCoursesPageNumber = availableCoursesPagedList.PageNumber;
        }
        private void MaterialsResponsePreparing(int courseId, int skillPage = 1, int materialPage = 1)
        {
            var course = _courseService.GetCourseAsync(courseId).GetAwaiter().GetResult();
            
            ViewBag.MaterialItems = course.MaterialList;
            ViewBag.CourseiId = courseId;
        }
    }
}
