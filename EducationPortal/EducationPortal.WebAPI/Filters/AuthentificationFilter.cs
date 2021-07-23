using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPortal.WebAPI.Filters
{
    public class AuthentificationFilter: ActionFilterAttribute
    {
        private ITokenService _tokenService;
        public AuthentificationFilter(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Query["token"];
            if (!await _tokenService.VerifyToken(token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
