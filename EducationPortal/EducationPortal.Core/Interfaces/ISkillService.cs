using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EducationPortal.Core
{
    public interface ISkillService
    {
        public Task<bool> AddSkillAsync(Skill skill, CancellationToken cancellationToken = default);
        public Task<bool> ChangeSkillAsync(Skill skill, CancellationToken cancellationToken = default);
        public Task<PagedList<Skill>> GetSkillsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        public Task<Skill> GetSkillAsync(int id, CancellationToken cancellationToken = default);
    }
}
