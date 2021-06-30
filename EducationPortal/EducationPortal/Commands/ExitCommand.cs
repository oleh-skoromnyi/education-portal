using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.UI
{
    class ExitCommand: ICommand
    {
        public bool IsAvailable()
        {
            return true;
        }

        public void Execute()
        {
            Environment.Exit(0);
        }
    }
}
