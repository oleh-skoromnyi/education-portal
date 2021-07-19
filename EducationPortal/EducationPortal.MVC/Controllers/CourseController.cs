using EducationPortal.Core;
using EducationPortal.MVC.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPortal.MVC.Controllers
{
    public class CourseController : Controller
    {
        private IUserService _userService;
        private ICourseService _courseService;
        private ISkillService _skillService;
        private IMaterialService _materialService;
        private IValidator<CourseViewModel> _courseValidator;
        private IValidator<AdditionViewModel> _additionValidator;
        private IConfiguration _config;

        public CourseController(ICourseService courseService, IValidator<CourseViewModel> courseValidator,
            IValidator<AdditionViewModel> additionValidator, IConfiguration config,
            ISkillService skillService, IMaterialService materialService, IUserService userService)
        {
            _userService = userService;
            _courseService = courseService;
            _skillService = skillService;
            _materialService = materialService;
            _courseValidator = courseValidator;
            _additionValidator = additionValidator;
            _config = config;
        }

        public IActionResult Index()
        {
            ResponsePreparing();
            return View();
        }

        public IActionResult Course(int courseId)
        {
            CourseResponsePreparing(courseId);
            return View();
        }
        public IActionResult CreateCourse(CourseViewModel course)
        {
            var validate = _courseValidator.Validate(course);
            if (validate.IsValid)
            {
                var result = false;
                result = _courseService.AddCourseAsync(int.Parse(this.Request.Cookies["UserId"]),
                    new Course
                    {
                        Name = course.Name,
                        Description = course.Description,
                        IsPublished = false
                    }).GetAwaiter().GetResult();
                if (result)
                {
                    ViewBag.SuccessMessage = "Course created successfully";
                }
                else
                {
                    ViewBag.ErrorMessage = "Course creating failed";
                }
            }
            else
            {
                ViewBag.Errors = validate.Errors;
            }
            ResponsePreparing();
            return View("Index");
        }
        public IActionResult AddAdditions(AdditionViewModel add, int courseId)
        {
            var validate = _additionValidator.Validate(add);
            if (validate.IsValid)
            {
                bool result = false;
                switch (add.AdditionType)
                {
                    case "givenSkill":
                        result = _courseService.AddSkillsToCourseAsync(
                            int.Parse(this.Request.Cookies["UserId"]), add.AdditionId,
                            courseId).GetAwaiter().GetResult();
                        break;
                    case "requirenmentSkill":
                        result = _courseService.AddRequirenmentToCourseAsync(
                            int.Parse(this.Request.Cookies["UserId"]),
                            new RequirenmentSkill { CourseId = courseId, Level = add.Level, SkillId = add.AdditionId },
                            courseId).GetAwaiter().GetResult();
                        break;
                    case "material":
                        result = _courseService.AddMaterialToCourseAsync(
                            int.Parse(this.Request.Cookies["UserId"]), add.AdditionId,
                            courseId).GetAwaiter().GetResult();
                        break;
                }
                if (result)
                {
                    ViewBag.SuccessMessage = "Addition added successfully";
                }
                else
                {
                    ViewBag.ErrorMessage = "Addition adding failed";
                }
            }
            else
            {
                ViewBag.Errors = validate.Errors;
            }
            CourseResponsePreparing(courseId);
            return View("Course");
        }
        public IActionResult PublishCourse(int courseId)
        {
            var result = _courseService.PublishCourseAsync(int.Parse(this.Request.Cookies["UserId"]), courseId).GetAwaiter().GetResult();
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
        public IActionResult ChangePage(int page)
        {
            if (page > 0)
            {
                ResponsePreparing(page);
            }
            else
            {
                ResponsePreparing();
            }
            return View("Index");
        }

        [HttpPost]
        public IActionResult ChangePageInCourse(int courseId, int skillPage,int materialPage)
        {
            if (skillPage > 1)
            {
                materialPage = 1;
            }
            else
            {
                skillPage = 1;
            }
            if (skillPage > 0 && materialPage > 0)
            {
                CourseResponsePreparing(courseId,skillPage,materialPage);
            }
            else
            {
                CourseResponsePreparing(courseId);
            }
            return View("Course");
        }
        private void ResponsePreparing(int page = 1)
        {
            var coursePagedList = _userService.GetCreatedNotPublishedCoursesAsync(
                int.Parse(this.Request.Cookies["UserId"]), 
                page, _config.GetValue<int>("PageSize"))
                .GetAwaiter().GetResult();
            ViewBag.PageCount = coursePagedList.PageCount;
            ViewBag.Items = coursePagedList.Items;
            ViewBag.PageNumber = coursePagedList.PageNumber;
        }
        private void CourseResponsePreparing(int courseId, int skillPage=1, int materialPage=1)
        {
            var course = _courseService.GetCourseAsync(courseId).GetAwaiter().GetResult();
            var skillPagedList = _skillService.GetSkillsAsync(skillPage, _config.GetValue<int>("PageSize")).GetAwaiter().GetResult();
            var materialPagedList = _materialService.GetMaterialsAsync(materialPage, _config.GetValue<int>("PageSize")).GetAwaiter().GetResult();

            ViewBag.CourseId = courseId;

            ViewBag.SkillPageCount = skillPagedList.PageCount;
            ViewBag.SkillItems = skillPagedList.Items;
            ViewBag.SkillPageNumber = skillPagedList.PageNumber;

            ViewBag.MaterialPageCount = materialPagedList.PageCount;
            ViewBag.MaterialItems = materialPagedList.Items;
            ViewBag.MaterialPageNumber = materialPagedList.PageNumber;
        }
    }
}
