using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using EducationPortal.MVC.Models;

namespace EducationPortal.Core.Validation
{
    public class AdditionViewModelValidator : AbstractValidator<AdditionViewModel>
    {
        public AdditionViewModelValidator()
        {
            RuleFor(obj => obj.AdditionId)
                .NotEmpty();
            RuleFor(obj => obj.AdditionType)
                .NotEmpty();
            RuleFor(obj => obj.Level)
                .NotEmpty().GreaterThan(0).When(x=>x.AdditionType == "requirenmentSkill");
        }
    }
}
