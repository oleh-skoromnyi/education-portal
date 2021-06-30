using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.UI
{
    public class State
    {
        public enum ProgramState {guest,authorized,userMenu,skillMenu,materialMenu,courseMenu}
        public ProgramState CurrentState{get;set;}
    }
}
