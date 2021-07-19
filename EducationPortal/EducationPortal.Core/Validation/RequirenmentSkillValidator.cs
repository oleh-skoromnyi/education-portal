using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Core.Validation
{
    public class RequirenmentSkillValidator : AbstractValidator<RequirenmentSkill>
    {
        public RequirenmentSkillValidator()
        {
            RuleFor(obj => obj.CourseId)
                .GreaterThan(0)
                .NotEmpty();
            RuleFor(obj => obj.SkillId)
                .GreaterThan(0)
                .NotEmpty();
            RuleFor(obj => obj.Level)
                .GreaterThan(0)
                .NotEmpty();
        }
    }
}
