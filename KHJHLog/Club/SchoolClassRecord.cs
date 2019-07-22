using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KHJHLog
{
    class SchoolClassRecord
    {
        public List<string> schoolIDList;

        public School school { get; set; }

        public List<OccurClass> classList { get; set; }

        public SchoolClassRecord()
        {
            schoolIDList = new List<string>();
            classList = new List<OccurClass>();
        }
    }
}
