using Autofac;
using EducationPortal.BusinessLogicLayer;
using EducationPortal.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;

namespace EducationPortal.UI
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            var userDb = new UserDb();
            var auth = new AuthService(userDb);
            var commandDictionary = new Dictionary<string, ICommand>();
            commandDictionary.Add("login", new LoginCommand(auth));
            commandDictionary.Add("register", new RegisterCommand(auth));
            commandDictionary.Add("logout", new LogoutCommand(auth));
            commandDictionary.Add("exit", new ExitCommand());
            builder.RegisterInstance(userDb).As<IDbContext<User>>();
            builder.RegisterInstance(auth).As<IAuth>();
            builder.RegisterInstance(commandDictionary);
            builder.RegisterType<CommandProcessor>();
            builder.RegisterType<CommandManager>();
            return builder.Build();
        }
    }
}
