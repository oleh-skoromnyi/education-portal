using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPortal.MVC.Models
{
    public enum MaterialTypeEnum {Article,Book,Video};
    public class MaterialViewModel
    {
        public MaterialTypeEnum MaterialType { get; set; }
        public string Name { get; set; }
        public string LinqToResource { get; set; }
        public DateTime DateOfPublication { get; set; }
        public string Authors { get; set; }
        public int Pages { get; set; }
        public int YearOfPublication { get; set; }
        public string FileExtension { get; set; }
        public string Quality { get; set; }
        public string Length { get; set; }
    }
}
