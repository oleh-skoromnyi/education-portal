using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace EducationPortal.Core.Validation
{
    public class UserSkillValidator : AbstractValidator<UserSkill>
    {
        public UserSkillValidator()
        {
            RuleFor(obj => obj.UserId)
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
