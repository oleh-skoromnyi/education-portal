using System;
using System.Collections.Generic;
using EducationPortal.UI;
using Autofac;
using EducationPortal.BusinessLogicLayer;

namespace EducationPortal
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ContainerConfig.Configure();
            using (var scope = container.BeginLifetimeScope()) 
            {
                var commandManager = scope.Resolve<CommandManager>();
                commandManager.Start();
            }
        }
    }
}
