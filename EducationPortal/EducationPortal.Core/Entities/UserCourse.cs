using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Core
{
    public class UserCourse
    {
        public int Progress { get; set; }
        public bool IsComplete { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public virtual Course Course{ get; set; }
        public virtual User User { get; set; }

        public UserCourse()
        {

        }

        public UserCourse(Course entity)
        {
            this.Course = entity;
            this.CourseId = entity.Id;
            this.IsComplete = false;
            this.Progress = 0;
        }
    }
}
