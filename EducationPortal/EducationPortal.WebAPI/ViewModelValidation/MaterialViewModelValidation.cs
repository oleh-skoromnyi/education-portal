using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using EducationPortal.MVC.Models;

namespace EducationPortal.Core.Validation
{
    public class MaterialViewModelValidation : AbstractValidator<MaterialViewModel>
    {
        public MaterialViewModelValidation()
        {
            RuleFor(obj => obj.MaterialType)
                .NotEmpty();
            RuleFor(obj => obj.Name)
                .MaximumLength(128)
                .NotEmpty();

            RuleFor(obj => obj.LinqToResource)
                .MaximumLength(1024)
                .NotEmpty().When(x => x.MaterialType == MaterialTypeEnum.Article);
            RuleFor(obj => obj.DateOfPublication)
                .NotEmpty().When(x => x.MaterialType == MaterialTypeEnum.Article);

            RuleFor(obj => obj.Authors)
                .NotEmpty().When(x => x.MaterialType == MaterialTypeEnum.Book);
            RuleFor(obj => obj.Pages)
                .NotEmpty().When(x => x.MaterialType == MaterialTypeEnum.Book);
            RuleFor(obj => obj.YearOfPublication)
                .NotEmpty().When(x => x.MaterialType == MaterialTypeEnum.Book);
            RuleFor(obj => obj.FileExtension)
                .NotEmpty().When(x => x.MaterialType == MaterialTypeEnum.Book);

            RuleFor(obj => obj.Quality)
                .NotEmpty().When(x => x.MaterialType == MaterialTypeEnum.Video);
            RuleFor(obj => obj.Length)
                .NotEmpty().When(x => x.MaterialType == MaterialTypeEnum.Video);
        }
    }
}
