using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Core
{
    public class CourseGivenSkill
    {
        public int SkillId { get; set; }
        public int CourseId { get; set; }
        public virtual Skill Skill { get; set; }
        public virtual Course Course { get; set; }

        public CourseGivenSkill()
        {

        }

        public CourseGivenSkill(Skill skill)
        {
            Skill = skill;
        }
    }
}