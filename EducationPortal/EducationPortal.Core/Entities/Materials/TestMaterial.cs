using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Core
{ 
    public class TestMaterial : Material
    {
        public List<TestItem> Questions { get; set; }

        public TestMaterial()
        {
            Questions = new List<TestItem>();
        }
    }
}
