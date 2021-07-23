using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPortal.MVC.Models
{
    public enum AdditionTypeEnum { GivenSkill, RequirenmentSkill, Material };
    public class AdditionViewModel
    {
        public int AdditionId { get; set; }
        public AdditionTypeEnum AdditionType { get; set; }
        public int Level { get; set; }
    }
}
