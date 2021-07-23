using System;
using System.Collections.Generic;
using EducationPortal.Core;
using System.Text;

namespace EducationPortal.UI.Commands
{
    public class SkillMenuGetSkillsCommand : ICommand
    {
        public string Name { get; } = "Get skills";

        private ISkillService service;
        private PageSettings pageSettings;

        public SkillMenuGetSkillsCommand(ISkillService service, PageSettings pageSettings)
        {
            this.service = service;
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
                var skillList = service.GetSkillsAsync(pageNumber, pageSettings.PageSize).GetAwaiter().GetResult();
                foreach (var skill in skillList.Items)
                {
                    Console.WriteLine($"{skill.Id}.{skill.Name}");
                    Console.WriteLine($"{skill.Description}");
                }
                Console.WriteLine("\nInput any number to exit:");
                Console.WriteLine($"n-to next page |{pageNumber}/{skillList.PageCount}|p-to previous page");
                var inputString = Console.ReadLine();
                if (inputString.Trim().ToLower() == "n" && pageNumber < skillList.PageCount)
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

        public bool IsAvailable(State state)
        {
            return state.CurrentState == State.ProgramState.skillMenu;
        }
    }
}
