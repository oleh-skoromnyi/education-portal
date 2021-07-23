using EducationPortal.Core;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPortal.MVC.Controllers
{
    public class SkillController : Controller
    {
        private ISkillService _skillService;
        private IValidator<Skill> _skillValidator;
        private IConfiguration _config;

        public SkillController(ISkillService skillService, IValidator<Skill> skillValidator, IConfiguration config)
        {
            _skillService = skillService;
            _skillValidator = skillValidator;
            _config = config;
        }

        public IActionResult Index()
        {
            ResponsePreparing();
            return View();
        }

        [HttpPost]
        public IActionResult ChangePage(int page)
        {
            if (page>0)
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
        public IActionResult AddSkill(Skill skill)
        {
            var validator = _skillValidator.Validate(skill);
            if (validator.IsValid)
            {
                if (_skillService.AddSkillAsync(skill).GetAwaiter().GetResult())
                {
                    ViewBag.SuccessMessage = "You successfully added skill";
                }
                else
                {
                    ViewBag.ErrorMessage = "Skill adding failed";
                }
            }
            else
            {
                ViewBag.Errors = validator.Errors;
            }
            ResponsePreparing();
            return View("Index");
        }
        private void ResponsePreparing(int page = 1)
        {
            var skillPagedList = _skillService.GetSkillsAsync(page, _config.GetValue<int>("PageSize")).GetAwaiter().GetResult();
            ViewBag.PageCount = skillPagedList.PageCount;
            ViewBag.Items = skillPagedList.Items;
            ViewBag.PageNumber = skillPagedList.PageNumber;
        }
    }
}
