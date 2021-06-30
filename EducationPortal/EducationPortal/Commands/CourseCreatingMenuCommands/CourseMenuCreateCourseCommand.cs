using System;
using System.Collections.Generic;
using EducationPortal.Core;
using System.Text;

namespace EducationPortal.UI.Commands
{
    public class CourseMenuCreateCourseCommand : ICommand
    {
        public string Name { get; } = "Create new course";

        private ICourseService courseService;

        public CourseMenuCreateCourseCommand(ICourseService courseService)
        {
            this.courseService = courseService;
        }
        public void Execute(ref State state, ref int userId)
        {
            Console.WriteLine("Input name:");
            var courseName = Console.ReadLine();
            Console.WriteLine("Input description of course:");
            var courseDescription = Console.ReadLine();
            var addedCourse = new Course
            {
                Name = courseName,
                Description = courseDescription,
                MaterialList = new List<CourseMaterial>(),
                GivenSkillList = new List<CourseGivenSkill>(),
                RequirementSkillList = new List<RequirenmentSkill>()
            };
            if (courseService.AddCourse(userId, addedCourse))
            {
                Console.WriteLine("Course added successful");
            }
            else
            {
                Console.WriteLine("Course adding failed");
            }
            Console.ReadLine();
        }

        public bool IsAvailable(State state)
        {
            return state.CurrentState == State.ProgramState.courseMenu;
        }
    }
}
