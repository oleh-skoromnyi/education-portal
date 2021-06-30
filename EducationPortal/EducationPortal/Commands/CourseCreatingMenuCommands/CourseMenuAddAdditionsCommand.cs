using System;
using System.Collections.Generic;
using EducationPortal.Core;
using System.Text;
using System.Linq;

namespace EducationPortal.UI.Commands
{
    public class CourseMenuAddAdditionsCommand : ICommand
    {
        public string Name { get; } = "Add additions to course";

        private ISkillService skillService;
        private ICourseService courseService;
        private IMaterialService materialService;
        private PageSettings pageSettings;

        public CourseMenuAddAdditionsCommand(ISkillService skillService, ICourseService courseService, IMaterialService materialService, PageSettings pageSettings)
        {
            this.skillService = skillService;
            this.courseService = courseService;
            this.materialService = materialService;
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
                var collection = courseService.GetCourses(pageNumber, pageSettings.PageSize);
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
                Console.WriteLine("\nInput id course you want to change:");
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
            var course = courseService.GetCourse(input);
            if (input != -1 && course != null)
            {
                string inputString = "";
                while (inputString != "exit")
                {
                    Console.Clear();
                    Console.WriteLine("Write exit to complete course changing");
                    Console.WriteLine("1. Add material to course");
                    Console.WriteLine("2. Add getting skills to course");
                    Console.WriteLine("3. Add requirenment skills to course");
                    inputString = Console.ReadLine();
                    int action;
                    int.TryParse(inputString, out action);
                    int id = -1;
                    switch (action)
                    {
                        case 1:
                            Console.WriteLine("Choose material to add(+ means already added)");
                            var materialList = materialService.GetMaterials(pageSettings.PageNumber,pageSettings.PageSize).Items.ToList();
                            Console.WriteLine(string.Join('\n', materialList.Select(x => $"   {(course.MaterialList.Any(y => y.MaterialId == x.Id) ? '+' : '-')}{x.Id}::{x.Name}")));
                            inputString = Console.ReadLine();
                            int.TryParse(inputString, out id);
                            if (id != 0 && id - 1 < materialList.Count())
                            {
                                if (!course.MaterialList.Contains(new CourseMaterial(materialList[id - 1])))
                                {
                                    if (courseService.AddMaterialToCourse(userId, materialList[id - 1], course.Id))
                                    {
                                        course = courseService.GetCourse(course.Id);
                                        Console.WriteLine("Material to course added successful");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Material to course adding failed");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Material already added.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Incorrect id.");
                            }
                            break;
                        case 2:
                            Console.WriteLine("Choose Skill to add(+ means already added)");
                            var skillList = skillService.GetSkills(pageSettings.PageNumber,pageSettings.PageSize).Items.ToList();
                            Console.WriteLine(string.Join('\n', skillList.Select(x => $"   {(course.GivenSkillList.Any(y => y.SkillId == x.Id) ? '+' : '-')}{x.Id}::{x.Name}")));
                            inputString = Console.ReadLine();
                            int.TryParse(inputString, out id);
                            if (id != 0 && id - 1 < skillList.Count())
                            {
                                if (!course.GivenSkillList.Contains(new CourseGivenSkill(skillList[id - 1])))
                                {
                                    if (courseService.AddSkillsToCourse(userId, skillList[id - 1], course.Id))
                                    {
                                        course = courseService.GetCourse(course.Id);
                                        Console.WriteLine("Skill to course added successful");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Skill to course adding failed");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Skill already added.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Incorrect id.");
                            }
                            break;
                        case 3:
                            Console.WriteLine("Choose Requirenment skill to add(+ means already added)");
                            var requirenmentsList = skillService.GetSkills(pageSettings.PageNumber, pageSettings.PageSize).Items.ToList();
                            Console.WriteLine(string.Join('\n', requirenmentsList.Select(x => $"   {(course.RequirementSkillList.Any(y => x.Id == y.Skill.Id) ? '+' : '-')}{x.Id}::{x.Name}")));
                            inputString = Console.ReadLine();
                            int.TryParse(inputString, out id);
                            if (id != 0 && id - 1 < requirenmentsList.Count())
                            {
                                if (!course.RequirementSkillList.Any(x => x.SkillId == id))
                                {
                                    var skillWithLevel = new RequirenmentSkill(requirenmentsList[id - 1]);
                                    Console.WriteLine("Write requirenment level of skill(+ means already added)");
                                    int level = -1;
                                    inputString = Console.ReadLine();
                                    int.TryParse(inputString, out level);
                                    if (level != -1)
                                    {
                                        skillWithLevel.Level = level;
                                        if (courseService.AddRequirenmentToCourse(userId, skillWithLevel, course.Id))
                                        {
                                            course = courseService.GetCourse(course.Id);
                                            Console.WriteLine("Skill requirenments added successful");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Skill requirenments adding failed");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Incorrect level.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Skill already added.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Incorrect id.");
                            }
                            break;
                        default:
                            if (inputString != "exit")
                            {
                                Console.WriteLine("Incorrect action");
                            }
                            break;
                    }
                }
            }
        }

        public bool IsAvailable(State state)
        {
            return state.CurrentState == State.ProgramState.courseMenu;
        }
    }
}
