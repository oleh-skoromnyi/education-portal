using EducationPortal.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EducationPortal.UI.Commands
{
    public class MaterialMenuAddBookMaterialCommand : ICommand
    {
        public string Name { get; } = "Add book";

        private IMaterialService service;

        public MaterialMenuAddBookMaterialCommand(IMaterialService service)
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
            var bookName = Console.ReadLine();
            Console.WriteLine("Input authors:");
            var authors = Console.ReadLine();
            Console.WriteLine("Input number of pages:");
            var pages = Console.ReadLine();
            Console.WriteLine("Input year of publication:");
            var yearOfPublication = Console.ReadLine();
            Console.WriteLine("Input file extension:");
            var format = Console.ReadLine();
            var addedMaterial = new DigitalBookMaterial
            {
                Name = bookName,
                Authors = authors,
                Pages = int.Parse(pages),
                YearOfPublication = int.Parse(yearOfPublication),
                Format = format
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
