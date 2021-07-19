using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace EducationPortal.Core.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(obj => obj.Name)
                .MaximumLength(128);
            RuleFor(obj => obj.Login)
                .MaximumLength(128)
                .NotEmpty();
            RuleFor(obj => obj.Password)
                .MaximumLength(1024)
                .NotEmpty();
            RuleFor(obj => obj.Email)
                .MaximumLength(128)
                .EmailAddress();
            RuleFor(obj => obj.Phone)
                .MaximumLength(128)
                .Matches(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$");
        }
    }
}
