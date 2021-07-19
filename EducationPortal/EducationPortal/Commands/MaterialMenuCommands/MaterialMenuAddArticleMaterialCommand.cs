using EducationPortal.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EducationPortal.UI.Commands
{
    public class MaterialMenuAddArticleMaterialCommand : ICommand
    {
        public string Name { get; } = "Add internet article";

        private IMaterialService service;

        public MaterialMenuAddArticleMaterialCommand(IMaterialService service)
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
            var articleName = Console.ReadLine();
            Console.WriteLine("Input linq to resource:");
            var linqToResource = Console.ReadLine();
            Console.WriteLine("Input date of publication:");
            var dateOfPublication = Console.ReadLine();
            DateTime date = default;
            if (DateTime.TryParse(dateOfPublication, out date))
            {
                var addedMaterial = new InternetArticleMaterial
                {
                    Name = articleName,
                    LinqToResource = linqToResource,
                    DateOfPublication = date
                };
                if (service.AddMaterialAsync(addedMaterial).GetAwaiter().GetResult())
                {
                    Console.WriteLine("Material added successful");
                    Console.ReadLine();
                    return;
                }
            }
            Console.WriteLine("Material adding failed");
            Console.ReadLine();
        }
    }
}
