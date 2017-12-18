using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmbiBotApp.Model
{
    public class Testing
    {
        public Record[] record { get; set; }
    }

    public class Record
    {
        public string Serial { get; set; }
        public string Statements { get; set; }
        public string Options { get; set; }
        public string Answers { get; set; }
        public string type { get; set; }
    }
}