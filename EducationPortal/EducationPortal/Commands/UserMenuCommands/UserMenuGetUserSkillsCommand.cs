using System;
using EducationPortal.Core;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EducationPortal.UI.Commands
{
    public class UserMenuGetUserSkillsCommand : ICommand
    {
        public string Name { get; } = "Get my skills";

        private IUserService service;
        private ISkillService skillService;
        private PageSettings pageSettings;

        public UserMenuGetUserSkillsCommand(IUserService service, ISkillService skillService, PageSettings pageSettings)
        {
            this.service = service;
            this.skillService = skillService;
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
                var userSkills = service.GetSkills(userId, pageNumber, pageSettings.PageSize);
                foreach (var skill in userSkills.Items)
                {
                    var skillData = skillService.GetSkill(skill.SkillId);
                    Console.WriteLine($"{skillData.Id}.{skillData.Name}");
                    Console.WriteLine($"{skillData.Description}");
                    Console.WriteLine($"Level : {skill.Level}");
                }
                Console.WriteLine("\nInput any number except 0 to exit:");
                Console.WriteLine($"n-to next page |{pageNumber}/{userSkills.PageCount}|p-to previous page");
                var inputString = Console.ReadLine();
                if (inputString.Trim().ToLower() == "n" && pageNumber < userSkills.PageCount)
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
            return state.CurrentState == State.ProgramState.userMenu;
        }
    }
}
