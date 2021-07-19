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
    public class MaterialController : Controller
    {
        private IMaterialService _materialService;
        private IValidator<MaterialViewModel> _materialValidator;
        private IConfiguration _config;

        public MaterialController(IMaterialService materialService, IValidator<MaterialViewModel> materialValidator, IConfiguration config)
        {
            _materialService = materialService;
            _materialValidator = materialValidator;
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
        public IActionResult AddMaterial(MaterialViewModel material)
        {
            var validator = _materialValidator.Validate(material);
            bool result = false;
            if (validator.IsValid)
            {
                switch (material.MaterialType)
                {
                    case "article":
                        result = _materialService.AddMaterialAsync(new InternetArticleMaterial
                        {
                            Name = material.Name,
                            LinqToResource = material.LinqToResource,
                            DateOfPublication = material.DateOfPublication
                        }).GetAwaiter().GetResult() ;
                        break;
                    case "book":
                        result = _materialService.AddMaterialAsync(new DigitalBookMaterial
                        {
                            Name = material.Name,
                            Authors = material.Authors,
                            Pages = material.Pages,
                            YearOfPublication = material.YearOfPublication,
                            Format = material.FileExtension
                        }).GetAwaiter().GetResult();
                        break;
                    case "video":
                        result = _materialService.AddMaterialAsync(new VideoMaterial
                        {
                            Name = material.Name,
                            Length = material.Length,
                            Quality = material.Quality
                        }).GetAwaiter().GetResult();
                        break;
                }
                if (result)
                {
                    ViewBag.SuccessMessage = "You successfully added material";
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
            var materialPagedList = _materialService.GetMaterialsAsync(page, _config.GetValue<int>("PageSize")).GetAwaiter().GetResult();
            ViewBag.PageCount = materialPagedList.PageCount;
            ViewBag.Items = materialPagedList.Items;
            ViewBag.PageNumber = materialPagedList.PageNumber;
        }
    }
}
