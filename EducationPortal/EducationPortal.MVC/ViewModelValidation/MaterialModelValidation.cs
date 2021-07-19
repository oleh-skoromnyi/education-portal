using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using EducationPortal.MVC.Models;

namespace EducationPortal.Core.Validation
{
    public class MaterialModelValidation : AbstractValidator<MaterialViewModel>
    {
        public MaterialModelValidation()
        {
            RuleFor(obj => obj.MaterialType)
                .MaximumLength(128)
                .NotEmpty();
            RuleFor(obj => obj.Name)
                .MaximumLength(128)
                .NotEmpty();

            RuleFor(obj => obj.LinqToResource)
                .MaximumLength(1024)
                .NotEmpty().When(x => x.MaterialType == "article");
            RuleFor(obj => obj.DateOfPublication)
                .NotEmpty().When(x => x.MaterialType == "article");

            RuleFor(obj => obj.Authors)
                .NotEmpty().When(x => x.MaterialType == "book");
            RuleFor(obj => obj.Pages)
                .NotEmpty().When(x => x.MaterialType == "book");
            RuleFor(obj => obj.YearOfPublication)
                .NotEmpty().When(x => x.MaterialType == "book");
            RuleFor(obj => obj.FileExtension)
                .NotEmpty().When(x => x.MaterialType == "book");

            RuleFor(obj => obj.Quality)
                .NotEmpty().When(x => x.MaterialType == "video");
            RuleFor(obj => obj.Length)
                .NotEmpty().When(x => x.MaterialType == "video");
        }
    }
}
