using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmbiBotApp.Model
{
    public class Structure
    {
        public string _id { get; set; }
        public TestStructure teststructure { get; set; }
    }

    public class TestStructure
    {
        public Courses[] courses { get; set; }
    }

    public class Courses
    {
        public string course_name { get; set; }
        public string total_tracks { get; set; }
        public Track[] Tracks { get; set; }
    }

    public class Track
    {
        public string track_name { get; set; }
        public string total_modules { get; set; }
        public Module[] Modules { get; set; }
    }

    public class Module
    {
        public string module_no { get; set; }
        public string module_name { get; set; }
        public string total_tech { get; set; }
        public Technolog[] Technologies { get; set; }
    }
    public class Technolog
    {
        public string tech_no { get; set; }
        public string tech_name { get; set; }
    }

}