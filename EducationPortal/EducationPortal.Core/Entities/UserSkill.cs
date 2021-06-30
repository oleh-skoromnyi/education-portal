using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Core
{
    public class UserSkill
    {
        public int Level { get; set; }
        public int SkillId { get; set; }
        public int UserId { get; set; }
        public virtual Skill Skill { get; set; }
        public virtual User User { get; set; }

        public UserSkill()
        {

        }

        public UserSkill(Skill skill)
        {
            this.Skill = skill;
            this.Level = 1;
        }
    }
}
