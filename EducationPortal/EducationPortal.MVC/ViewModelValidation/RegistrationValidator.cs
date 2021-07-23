using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using EducationPortal.MVC.Models;

namespace EducationPortal.Core.Validation
{
    public class RegistrationValidator : AbstractValidator<RegistrationViewModel>
    {
        public RegistrationValidator()
        {
            RuleFor(obj => obj.Name)
                .MaximumLength(128);
            RuleFor(obj => obj.Login)
                .MaximumLength(128)
                .NotEmpty();
            RuleFor(obj => obj.Password)
                .MaximumLength(1024)
                .NotEmpty();
            RuleFor(obj => obj.ConfirmPassword)
                .Equal(x=>x.Password).WithMessage(@"'Confirm Password' должно быть равно 'Password'.");
            RuleFor(obj => obj.Email)
                .MaximumLength(128)
                .EmailAddress();
            RuleFor(obj => obj.Phone)
                .MaximumLength(128)
                .Matches(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$");
        }
    }
}
