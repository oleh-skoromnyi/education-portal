using System;
using EducationPortal.Core;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EducationPortal.UI.Commands
{
    public class UserMenuStartCourseCommand : ICommand
    {
        public string Name { get; } = "Start new course";

        private IUserService userService;
        private ICourseService courseService;
        private PageSettings pageSettings;

        public UserMenuStartCourseCommand(IUserService userService,ICourseService courseService, PageSettings pageSettings)
        {
            this.userService = userService;
            this.courseService = courseService;
            this.pageSettings = pageSettings;
        }
        public void Execute(ref State state, ref int userId)
        { 
            int userIdTemp = userId;
            int input = 0;
            int pageNumber = pageSettings.PageNumber;
            while (input == 0)
            {
                Console.Clear();
                var courses = userService.GetAvailableCoursesAsync(userId, pageNumber, pageSettings.PageSize).GetAwaiter().GetResult();
                foreach (var availableCourse in courses.Items)
                {
                    Console.WriteLine($"{availableCourse.Id}.{availableCourse.Name}");
                    Console.WriteLine($"{availableCourse.Description}");
                }
                Console.WriteLine("\nInput course id(b-to go back):");
                Console.WriteLine($"n-to next page |{pageNumber}/{courses.PageCount}|p-to previous page");
                var inputString = Console.ReadLine();
                int.TryParse(inputString, out input);
                if (inputString.Trim().ToLower() == "b")
                {
                    input = -1;
                }
                else
                {
                    if (inputString.Trim().ToLower() == "n" && pageNumber < courses.PageCount)
                    {
                        pageNumber++;
                    }
                    else
                    {
                        if (inputString.Trim().ToLower() == "p" && pageNumber > 1)
                        {
                            pageNumber--;
                        }
                        else
                        {
                            if (input != 0)
                            {
                                if (userService.StartLearnCourseAsync(userId, input).GetAwaiter().GetResult())
                                {
                                    Console.WriteLine($"You start course");
                                }
                                else
                                {
                                    Console.WriteLine($"You not start course");
                                }
                            }
                        }
                    }
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
