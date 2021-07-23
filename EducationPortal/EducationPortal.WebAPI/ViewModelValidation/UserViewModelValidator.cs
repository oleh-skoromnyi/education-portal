using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.MVC.Models;
using FluentValidation;

namespace EducationPortal.Core.Validation
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        public UserViewModelValidator()
        {
            RuleFor(obj => obj.Name)
                .MaximumLength(128);
            RuleFor(obj => obj.Email)
                .MaximumLength(128)
                .EmailAddress();
            RuleFor(obj => obj.Phone)
                .MaximumLength(128)
                .Matches(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$");
        }
    }
}   