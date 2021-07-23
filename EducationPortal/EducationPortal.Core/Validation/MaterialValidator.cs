using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace EducationPortal.Core.Validation
{
    public class MaterialValidator : AbstractValidator<Material>
    {
        public MaterialValidator()
        {
            RuleFor(obj => obj.Name)
                .NotEmpty()
                .MaximumLength(128);
        }
    }
}
