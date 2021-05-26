using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.UI
{
    public interface ICommand
    {
        public bool IsAvailable();

        public void Execute();
    }
}
