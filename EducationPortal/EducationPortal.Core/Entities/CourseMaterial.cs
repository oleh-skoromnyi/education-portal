using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Core
{
    public class CourseMaterial
    {
        public int MaterialId { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public virtual Material Material { get; set; }
        public CourseMaterial()
        {

        }
        public CourseMaterial(Material material)
        {
            Material = material;
        }
    }
}