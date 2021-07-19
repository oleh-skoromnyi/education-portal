using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Core
{
    public class DigitalBookMaterial : Material
    {
        public string Authors { get; set; }
        public int Pages { get; set; }
        public string Format { get; set; }
        public int YearOfPublication { get; set; }
    }
}
