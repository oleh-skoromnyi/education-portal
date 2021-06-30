using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Core
{
    public class Course : Entity
    {
        public int AuthorId { get; set; }
        public bool IsPublished { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CourseMaterial> MaterialList { get; set; }
        public List<CourseGivenSkill> GivenSkillList { get; set; }
        public List<RequirenmentSkill> RequirementSkillList { get; set; }
    }
}
