using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Core
{
    public class LearnedMaterial
    {
        public int UserId { get; set; }
        public int MaterialId { get; set; }
        public virtual User User { get; set; }
        public virtual Material Material { get; set; }
        public LearnedMaterial()
        {

        }
        public LearnedMaterial(Material material)
        {
            Material = material;
        }
    }
}
