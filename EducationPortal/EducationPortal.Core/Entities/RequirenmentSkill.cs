using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Core
{
    public class RequirenmentSkill
    {
        public int Level { get; set; }
        public int SkillId { get; set; }
        public int CourseId { get; set; }
        public virtual Skill Skill { get; set; }
        public virtual Course Course { get; set; }

        public RequirenmentSkill()
        {

        }

        public RequirenmentSkill(Skill skill)
        {
            this.Skill = skill;
            this.Level = 1;
        }
    }
}
