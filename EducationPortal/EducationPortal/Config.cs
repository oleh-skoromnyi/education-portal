using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.UI
{
    public class Config
    {
        public string EFConnectionString { get; set; }
        public string FileDBPathToUsers { get; set; }
        public string FileDBPathToSkills { get; set; }
        public string FileDBPathToCourses { get; set; }
        public string FileDBPathToArticlesMaterials { get; set; }
        public string FileDBPathToBooksMaterials { get; set; }
        public string FileDBPathToTestsMaterials { get; set; }
        public string FileDBPathToVideoMaterials { get; set; }
    }
}
