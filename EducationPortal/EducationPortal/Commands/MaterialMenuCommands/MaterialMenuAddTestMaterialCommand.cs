using EducationPortal.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EducationPortal.UI.Commands
{
    public class MaterialMenuAddTestMaterialCommand : ICommand
    {
        public string Name { get; } = "Add test";

        private IMaterialService service;

        public MaterialMenuAddTestMaterialCommand(IMaterialService service)
        {
            this.service = service;
        }

        public bool IsAvailable(State state)
        {
            return state.CurrentState == State.ProgramState.materialMenu;
        }

        public void Execute(ref State state, ref int userId)
        {
            Console.WriteLine("Input name:");
            var testName = Console.ReadLine();
            var questions = new List<TestItem>();
            string input = "";
            while (input != "exit")
            {
                Console.Clear();
                Console.WriteLine("Write exit to complete test creating(Only on write question request)");
                Console.WriteLine("Write question");
                input = Console.ReadLine();
                if (input != "exit")
                {
                    var question = input;
                    var answers = new List<Answer>();
                    int correct = 0;
                    Console.WriteLine("Write stop to complete test item creating");
                    Console.WriteLine("Write answers");
                    while (input != "stop")
                    {
                        input = Console.ReadLine();
                        if (input != "stop")
                        {
                            answers.Add(new Answer { AnswerString = input });
                        }
                        else
                        {
                            Console.WriteLine("Which answer was correct?(Write position starting from 1)");
                            input = Console.ReadLine();
                            correct = int.Parse(input) - 1;
                            break;
                        }
                    }
                    if (answers.Any() && correct < answers.Count)
                    {
                        questions.Add(new TestItem { Question = question, CorrectAnswerIndex = correct, Answers = answers });
                    }
                }
            }

            var addedMaterial = new TestMaterial
            {
                Name = testName,
                Questions = questions
            };
            if (service.AddMaterial(addedMaterial))
            {
                Console.WriteLine("Material added successful");
            }
            else
            {
                Console.WriteLine("Material adding failed");
            }
        }
    }
}
