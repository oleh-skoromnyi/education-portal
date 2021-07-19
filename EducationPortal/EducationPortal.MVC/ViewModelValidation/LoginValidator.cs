using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using EducationPortal.MVC.Models;

namespace EducationPortal.Core.Validation
{
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(obj => obj.Login)
                .MaximumLength(128)
                .NotEmpty();
            RuleFor(obj => obj.Password)
                .MaximumLength(1024)
                .NotEmpty();
        }
    }
}
