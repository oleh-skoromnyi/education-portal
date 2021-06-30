using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Core
{
    public interface ISkillService
    {
        public bool AddSkill(Skill skill);
        public bool ChangeSkill(Skill skill);
        public PagedList<Skill> GetSkills(int pageNumber, int pageSize);
        public Skill GetSkill(int id);
    }
}
