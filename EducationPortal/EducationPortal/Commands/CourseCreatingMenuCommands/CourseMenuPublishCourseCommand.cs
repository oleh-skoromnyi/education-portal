using System;
using System.Collections.Generic;
using EducationPortal.Core;
using System.Text;
using System.Linq;

namespace EducationPortal.UI.Commands
{
    public class CourseMenuPublishCourseCommand : ICommand
    {
        public string Name { get; } = "Publish course";

        private ICourseService courseService;
        private PageSettings pageSettings;

        public CourseMenuPublishCourseCommand(ICourseService courseService, PageSettings pageSettings)
        {
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
                var collection = courseService.GetCoursesAsync(pageNumber, pageSettings.PageSize).GetAwaiter().GetResult();
                foreach (var courseItem in collection.Items.Where(x => !x.IsPublished && x.AuthorId == userIdTemp))
                {
                    Console.WriteLine($"{courseItem.Id}.{courseItem.Name}");
                    Console.WriteLine($"{courseItem.Description}");
                    Console.WriteLine($"Material:");
                    if (courseItem.MaterialList.Any())
                    {
                        Console.WriteLine(string.Join('\n', courseItem.MaterialList.Select(x => $"   {x.Material.Id}::{x.Material.Name}")));
                    }
                    Console.WriteLine($"Given Skills:");
                    if (courseItem.GivenSkillList.Any())
                    {
                        Console.WriteLine(string.Join('\n', courseItem.GivenSkillList.Select(x => $"    {x.Skill.Id}::{x.Skill.Name}")));
                    }
                    Console.WriteLine($"Requirenments:");
                    if (courseItem.RequirementSkillList.Any())
                    {
                        Console.WriteLine(string.Join('\n', courseItem.RequirementSkillList.Select(x => $"   {x.Skill.Id}({x.Level})::{x.Skill.Name}")));
                    }
                }
                Console.WriteLine("\nInput id course you want to publish:");
                Console.WriteLine($"n-to next page |{pageNumber}/{collection.PageCount}|p-to previous page");
                var inputString = Console.ReadLine();
                if (inputString.Trim().ToLower() == "n" && pageNumber < collection.PageCount)
                {
                    pageNumber++;
                }
                else
                {
                    if (inputString.Trim().ToLower() == "p" && pageNumber > 1)
                    {
                        pageNumber--;
                    }
                }
                int.TryParse(inputString, out input);
            }
            if (courseService.PublishCourseAsync(userId, input).GetAwaiter().GetResult())
            {
                Console.WriteLine("Course publicated successfully");
            }
            Console.ReadLine();
        }

        public bool IsAvailable(State state)
        {
            return state.CurrentState == State.ProgramState.courseMenu;
        }
    }
}

