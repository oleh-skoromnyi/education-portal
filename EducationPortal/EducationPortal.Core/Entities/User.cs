using System.Collections.Generic;

namespace EducationPortal.Core
{
    public class User : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<UserSkill> SkillList { get; set; }
        public List<LearnedMaterial> MaterialList { get; set; }
        public List<UserCourse> CourseList { get; set; }
    }
}
