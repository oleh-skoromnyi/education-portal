using System.Threading;
using System.Threading.Tasks;
using EducationPortal.Core;
using FluentValidation;

namespace EducationPortal.BLL
{
    public class SkillService : ISkillService
    {
        private IRepository<Skill> _skillRepository;
        private IValidator<Skill> _skillValidator;

        public SkillService(IRepository<Skill> repos, IValidator<Skill> skillValidator)
        {
            this._skillRepository = repos;
            this._skillValidator = skillValidator;
        }
        public async Task<bool> AddSkillAsync(Skill skill, CancellationToken cancellationToken = default)
        {
            if ((await _skillValidator.ValidateAsync(skill, cancellationToken)).IsValid)
            {
                if (await _skillRepository.InsertAsync(skill, cancellationToken))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> ChangeSkillAsync(Skill skill, CancellationToken cancellationToken = default)
        {
            if ((await _skillValidator.ValidateAsync(skill, cancellationToken)).IsValid)
            {
                if (await _skillRepository.ExistAsync(new FindByIdSpecification<Skill>(skill.Id), cancellationToken))
                {
                    if (await _skillRepository.UpdateAsync(skill))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<Skill> GetSkillAsync(int skillId, CancellationToken cancellationToken = default)
        {
            var specification = new FindByIdSpecification<Skill>(skillId);
            return await _skillRepository.FindAsync(specification, cancellationToken);
        }

        public async Task<PagedList<Skill>> GetSkillsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var specification = new Specification<Skill>(x => true);
            return await _skillRepository.LoadListAsync(specification, pageNumber, pageSize, cancellationToken);
        }
    }
}
