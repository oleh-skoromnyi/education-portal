using EducationPortal.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EducationPortal.UI.Commands
{
    public class MaterialMenuAddVideoMaterialCommand : ICommand
    {
        public string Name { get; } = "Add video";

        private IMaterialService service;

        public MaterialMenuAddVideoMaterialCommand(IMaterialService service)
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
            var videoName = Console.ReadLine();
            Console.WriteLine("Input quality:");
            var quality = Console.ReadLine();
            Console.WriteLine("Input length:");
            var length = Console.ReadLine();
            var addedMaterial = new VideoMaterial
            {
                Name = videoName,
                Length = length,
                Quality = quality
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
