using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace KHJHLog
{
    class OccurClass
    {
        public OccurClass(XElement elmClass)
        {
            GradeYear = elmClass.ElementText("GradeYear");
            
            ClassID = elmClass.ElementText("ClassID");

            ClassName = elmClass.ElementText("ClassName");

            TeacherID = elmClass.ElementText("TeacherId");

            TeacherName = elmClass.ElementText("TeacherName");

        }

        public string GradeYear { get; set; }
        public string ClassID { get; set; }
        public string ClassName { get; set; }

        public string TeacherID { get; set; }
        public string TeacherName { get; set; }
    }
}
