using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Core
{
    public class TestItem : Entity
    {
        public string Question { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public List<Answer> Answers { get; set; }

        public TestItem()
        {
            Answers = new List<Answer>();
        }
    }
}
