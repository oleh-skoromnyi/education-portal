using System;
using System.Collections.Generic;
using System.Text;
using EducationPortal.Core;

namespace EducationPortal.BLL
{
    public class SkillService : ISkillService
    {
        private IRepository<Skill> _repository;

        public SkillService(IRepository<Skill> repos)
        {
            this._repository = repos;
        }
        public bool AddSkill(Skill skill)
        {
            if (skill != null)
            {
                if (_repository.FindIndex(skill.Name) == 0)
                {
                    if (_repository.Save(skill))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ChangeSkill(Skill skill)
        {
            if (_repository.FindIndex(skill.Name) != 0)
            {
                if (_repository.Update(skill))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public Skill GetSkill(int id)
        {
            var specification = new Specification<Skill>(x => x.Id == id);
            return _repository.Find(specification);
        }

        public PagedList<Skill> GetSkills(int pageNumber, int pageSize)
        {
            var specification = new Specification<Skill>(x => true);
            return _repository.LoadList(specification, pageNumber, pageSize);
        }
    }
}
