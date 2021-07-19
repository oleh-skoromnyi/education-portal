using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace EducationPortal.Core.Validation
{
    public class CourseValidator : AbstractValidator<Course>
    {
        public CourseValidator()
        {
            RuleFor(obj => obj.Name)
                .NotEmpty()
                .MaximumLength(128);
            RuleFor(obj => obj.Description)
                .MaximumLength(1024);
        }
    }
}
