using EducationPortal.Core;
using EducationPortal.MVC.Models;
using EducationPortal.WebAPI;
using EducationPortal.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EducationPortal.MVC.Controllers
{
    [ApiController]
    [Route("Material")]
    public class MaterialController : Controller
    {
        private IMaterialService _materialService;
        private ITokenService _tokenService;

        public MaterialController(IMaterialService materialService, ITokenService tokenService)
        {
            _materialService = materialService;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Route("GetMaterials")]
        [ServiceFilter(typeof(AuthentificationFilter))]
        public async Task<IActionResult> GetMaterials(string token, int pageNumber = 1, int pageSize = 10)
        {
            var materialPagedList = await _materialService.GetMaterialsAsync(pageNumber, pageSize);
            return new ObjectResult(materialPagedList);
        }

        [HttpPost]
        [Route("AddMaterial")]
        [ServiceFilter(typeof(AuthentificationFilter))]
        public async Task<IActionResult> AddMaterial(string token, MaterialViewModel material)
        {
            if (ModelState.IsValid)
            {
                var result = false;
                switch (material.MaterialType)
                {
                    case MaterialTypeEnum.Article:
                        result = await _materialService.AddMaterialAsync(new InternetArticleMaterial
                        {
                            Name = material.Name,
                            LinqToResource = material.LinqToResource,
                            DateOfPublication = material.DateOfPublication
                        });
                        break;
                    case MaterialTypeEnum.Book:
                        result = await _materialService.AddMaterialAsync(new DigitalBookMaterial
                        {
                            Name = material.Name,
                            Authors = material.Authors,
                            Pages = material.Pages,
                            YearOfPublication = material.YearOfPublication,
                            Format = material.FileExtension
                        });
                        break;
                    case MaterialTypeEnum.Video:
                        result = await _materialService.AddMaterialAsync(new VideoMaterial
                        {
                            Name = material.Name,
                            Length = material.Length,
                            Quality = material.Quality
                        });
                        break;
                    default:
                        break;
                }
                if (result)
                {
                    return new OkResult();
                }
            }
            return BadRequest();
        }
    }
}
