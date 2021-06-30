using System;
using EducationPortal.Core;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EducationPortal.UI.Commands
{
    public class UserMenuGetUserCoursesCommand : ICommand
    {
        public string Name { get; } = "Get my courses";

        private IUserService service;
        private ICourseService courseService;

        public UserMenuGetUserCoursesCommand(IUserService service, ICourseService courseService)
        {
            this.service = service;
            this.courseService = courseService;
        }
        public void Execute(ref State state, ref int userId)
        {
            var userData = service.GetPersonalData(userId);
            if (userData.CourseList != null)
            {
                foreach (var course in userData.CourseList)
                {
                    var courseData = courseService.GetCourse(course.CourseId);
                    Console.WriteLine($"{courseData.Id}.{courseData.Name}");
                    Console.WriteLine($"{courseData.Description}");
                    Console.WriteLine($"Complete : {(course.IsComplete ? '+' : '-')}");
                    Console.WriteLine($"Progress : {course.Progress}");
                }
            }
            Console.ReadLine();
        }

        public bool IsAvailable(State state)
        {
            return state.CurrentState == State.ProgramState.userMenu;
        }
    }
}
