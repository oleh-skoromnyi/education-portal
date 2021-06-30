using System;
using EducationPortal.Core;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EducationPortal.UI.Commands
{
    public class UserMenuCoursePassingCommand : ICommand
    {
        public string Name { get; } = "Go to course";

        private IUserService userService;
        private ICourseService courseService;
        private IMaterialService materialService;

        public UserMenuCoursePassingCommand(IUserService userService, ICourseService courseService, IMaterialService materialService)
        {
            this.userService = userService;
            this.courseService = courseService;
            this.materialService = materialService;
        }
        public UserMenuCoursePassingCommand(IUserService service)
        {
            this.userService = service;
        }
        public void Execute(ref State state, ref int userId)
        {
            int input;
            var userData = userService.GetPersonalData(userId);
            Console.WriteLine($"Write course id:");
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
            int.TryParse(Console.ReadLine(), out input);
            if (input > 0 && input <= userData.CourseList.Count)
            {
                CoursePassing(userId, input);
            }
        }

        public bool IsAvailable(State state)
        {
            return state.CurrentState == State.ProgramState.userMenu;
        }

        private void CoursePassing(int userId, int courseId)
        {
            Console.Clear();
            var course = courseService.GetCourse(courseId);
            string input = "";
            int materialId = -1;

            while (input != "exit")
            {
                Console.Clear();
                Console.WriteLine("Write material id(write exit to exit)");
                var user = userService.GetPersonalData(userId);

                if (course.MaterialList.Any())
                {
                    foreach (var material in course.MaterialList)
                    {
                        var materialData = materialService.GetMaterial(material.MaterialId);
                        Console.WriteLine($"{(user.MaterialList.Any(y => y.MaterialId == material.MaterialId) ? '+' : '-')}{material.MaterialId}::{materialData.Name}");
                    }
                }

                input = Console.ReadLine();
                if (input != "exit")
                {
                    int.TryParse(input, out materialId);

                    if (courseId != -1 && course.MaterialList.Any(x => x.MaterialId == materialId))
                    {
                        var material = materialService.GetMaterial(materialId);
                        Console.WriteLine($"{material.Id}::{material.Name}");

                        if (material is VideoMaterial)
                        {
                            var video = (VideoMaterial)material;
                            Console.WriteLine($"{video.Id}.{video.Name}\n" +
                                $"Quality: {video.Quality}\n" +
                                $"Length: {video.Length}");
                            Console.WriteLine($"\nWrite something when complete to learn material");
                            input = Console.ReadLine();
                            if (input != "")
                            {
                                userService.LearnMaterial(user.Id, video);
                            }
                        }
                        if (material is DigitalBookMaterial)
                        {
                            var book = (DigitalBookMaterial)material;
                            Console.WriteLine($"{book.Id}.{book.Name}\n" +
                                  $"Authors: {book.Authors}\n" +
                                  $"Pages: {book.Pages}\n" +
                                  $"Year of publication: {book.YearOfPublication}\n" +
                                  $"Format: {book.Format}");
                            Console.WriteLine($"\nWrite something when complete to learn material");
                            input = Console.ReadLine();
                            if (input != "")
                            {
                                userService.LearnMaterial(user.Id, book);
                            }
                        }
                        if (material is InternetArticleMaterial)
                        {
                            var article = (InternetArticleMaterial)material;
                            Console.WriteLine($"{article.Id}.{article.Name}\n" +
                                $"Linq to resource: {article.LinqToResource}\n" +
                                $"Date of publication: {article.DateOfPublication}");
                            Console.WriteLine($"\nWrite something when complete to learn material");
                            input = Console.ReadLine();
                            if (input != "")
                            {
                                userService.LearnMaterial(user.Id, article);
                            }
                        }
                        if (material is TestMaterial)
                        {
                            var test = (TestMaterial)material;
                            int index = -1;
                            int count = 0;
                            Console.WriteLine($"{test.Id}.{test.Name}");
                            foreach (var question in test.Questions)
                            {
                                Console.WriteLine($"Answer the question(write index of correct answer):");
                                Console.WriteLine($"{question.Question}");

                                for (int i = 0; i < question.Answers.Count(); i++)
                                {
                                    Console.WriteLine($"{i + 1}.{question.Answers[i].AnswerString}");
                                }
                                input = Console.ReadLine();
                                int.TryParse(input, out index);
                                if (index - 1 == question.CorrectAnswerIndex)
                                {
                                    count++;
                                }
                            }
                            if (count == test.Questions.Count())
                            {
                                userService.LearnMaterial(user.Id, test);
                            }
                        }
                    }
                }
            }
        }
    }
}