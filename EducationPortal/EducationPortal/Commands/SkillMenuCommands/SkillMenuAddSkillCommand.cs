using System;
using System.Collections.Generic;
using EducationPortal.Core;
using System.Text;

namespace EducationPortal.UI.Commands 
{
    class SkillMenuAddSkillCommand : ICommand
    {
        public string Name { get; } = "Add skill";

        private ISkillService service;

        public SkillMenuAddSkillCommand(ISkillService service)
        {
            this.service = service;
        }
        public void Execute(ref State state, ref int userId)
        {
            Console.WriteLine("Input name:");
            var skillName = Console.ReadLine();
            Console.WriteLine("Input description of skill:");
            var skillDescription = Console.ReadLine();
            var addedSkill = new Skill
            {
                Name = skillName,
                Description = skillDescription
            };
            if (service.AddSkillAsync(addedSkill).GetAwaiter().GetResult())
            {
                Console.WriteLine("Skill added successful");
            }
            else
            {
                Console.WriteLine("Skill adding failed");
            }
            Console.ReadLine();
        }

        public bool IsAvailable(State state)
        {
            return state.CurrentState == State.ProgramState.skillMenu;
        }
    }
}
