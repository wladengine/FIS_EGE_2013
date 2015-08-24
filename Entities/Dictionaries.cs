using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIS_EGE_2013
{
    public class CommonDictionary
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    //public class Dictionary01Subject : CommonDictionary { }
    //public class Dictionary02StudyLevel : CommonDictionary { }
    //public class Dictionary03OlympLevel : CommonDictionary { }
    //public class Dictionary04ApplicationStatus : CommonDictionary { }
    //public class Dictionary05Sex : CommonDictionary { }
    //public class Dictionary06MarkDocument : CommonDictionary { }
    //public class Dictionary07Country : CommonDictionary { }

    public class DictionaryOlympiad
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public string Subject { get; set; }
    }

    public class Dictionary10Direction
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string QualificationCode { get; set; }
    }

    //public class Dictionary11ExamType : CommonDictionary { }
    //public class Dictionary11ExamType : CommonDictionary { }
}
