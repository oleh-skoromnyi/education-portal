using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using EducationPortal.MVC.Models;

namespace EducationPortal.Core.Validation
{
    public class CourseViewModelValidator : AbstractValidator<CourseViewModel>
    {
        public CourseViewModelValidator()
        {
            RuleFor(obj => obj.Name)
                .MaximumLength(128)
                .NotEmpty();
            RuleFor(obj => obj.Description)
                .MaximumLength(1024)
                .NotEmpty();
        }
    }
}
