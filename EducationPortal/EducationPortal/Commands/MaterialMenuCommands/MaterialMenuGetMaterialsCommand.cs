using EducationPortal.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EducationPortal.UI.Commands
{
    public class MaterialMenuGetMaterialsCommand : ICommand
    {
        public string Name { get; } = "Get materials";

        private IMaterialService service;
        private PageSettings pageSettings;

        public MaterialMenuGetMaterialsCommand(IMaterialService service, PageSettings pageSettings)
        {
            this.service = service;
            this.pageSettings = pageSettings;
        }

        public bool IsAvailable(State state)
        {
            return state.CurrentState == State.ProgramState.materialMenu;
        }

        public void Execute(ref State state, ref int userId)
        {
            int userIdTemp = userId;
            int input = 0;
            int pageNumber = pageSettings.PageNumber;
            while (input == 0)
            {
                Console.Clear();
                var materialList = service.GetMaterialsAsync(pageNumber, pageSettings.PageSize).GetAwaiter().GetResult();
                var videoList = materialList.Items.OfType<VideoMaterial>();
                var articlesList = materialList.Items.OfType<InternetArticleMaterial>();
                var bookList = materialList.Items.OfType<DigitalBookMaterial>();
                var testList = materialList.Items.OfType<TestMaterial>();
                if (videoList.Any())
                {
                    Console.WriteLine($"Video materials:"); 
                    foreach (var material in videoList)
                    {
                        Console.WriteLine($"\t{material.Id}::{material.Name}::{material.Quality}::{material.Length}");
                    }
                }
                if (articlesList.Any())
                {
                    Console.WriteLine($"Internet articles:"); 
                    foreach (var material in articlesList)
                    {
                        Console.WriteLine($"\t{material.Id}::{material.Name}::{material.LinqToResource}::{material.DateOfPublication}");
                    }
                }
                if (bookList.Any())
                {
                    Console.WriteLine($"Digital books:");
                    foreach (var material in bookList)
                    {
                        Console.WriteLine($"\t{material.Id}::{material.Name}::{material.Authors}::{material.Pages}::{material.YearOfPublication}::{material.Format}");
                    }
                }
                if (testList.Any())
                {
                    Console.WriteLine($"Tests:");
                    foreach (var material in testList)
                    {
                        Console.WriteLine($"\t{material.Id}::{material.Name}");
                    }
                }
                Console.WriteLine("\nInput any number to exit:");
                Console.WriteLine($"n-to next page |{pageNumber}/{materialList.PageCount}|p-to previous page");
                var inputString = Console.ReadLine();
                if (inputString.Trim().ToLower() == "n" && pageNumber < materialList.PageCount)
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
        }
    }
}
