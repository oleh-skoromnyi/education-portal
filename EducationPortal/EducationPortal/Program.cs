using System;
using System.Collections.Generic;
using EducationPortal.UI;
using EducationPortal.BusinessLogic;
using EducationPortal.DataAccess;

namespace EducationPortal
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandDictionary = new Dictionary<string, ICommand>();
            FileDb database = new FileDb();
            Authorization auth = new Authorization(database);
            commandDictionary.Add("Login", new LoginCommand(auth));
            commandDictionary.Add("Register", new RegisterCommand(auth));
            //commandDictionary.Add("Logout", new LogoutCommand());
            var commandProcessor = new CommandProcessor(commandDictionary);
            var commandManager = new CommandManager(commandProcessor, auth);
            commandManager.Start();
        }
    }
}
