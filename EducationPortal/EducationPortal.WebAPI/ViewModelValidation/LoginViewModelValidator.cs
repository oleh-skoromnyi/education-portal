using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using EducationPortal.MVC.Models;

namespace EducationPortal.Core.Validation
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
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
