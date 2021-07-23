using EducationPortal.Core;
using EducationPortal.WebAPI;
using EducationPortal.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EducationPortal.MVC.Controllers
{
    [ApiController]
    [Route("Skill")]
    public class SkillController : Controller
    {
        private ISkillService _skillService;
        private ITokenService _tokenService;

        public SkillController(ISkillService skillService, ITokenService tokenService)
        {
            _skillService = skillService;
            _tokenService = tokenService;
        }


        [HttpGet]
        [Route("GetSkills")]
        [ServiceFilter(typeof(AuthentificationFilter))]
        public async Task<IActionResult> GetSkills(string token, int pageNumber = 1, int pageSize = 10)
        {
            if (await _tokenService.VerifyToken(token))
            {
                var skillPagedList = await _skillService.GetSkillsAsync(pageNumber, pageSize);
                return new ObjectResult(skillPagedList);
            }
            return BadRequest();
        }


        [HttpPost]
        [Route("AddSkill")]
        [ServiceFilter(typeof(AuthentificationFilter))]
        public async Task<IActionResult> AddSkill(string token, Skill skill)
        {
            if (await _tokenService.VerifyToken(token))
            {
                if (ModelState.IsValid)
                {
                    if (await _skillService.AddSkillAsync(skill))
                    {
                        return new OkResult();
                    }
                }
            }
            return BadRequest();
        }
    }
}
