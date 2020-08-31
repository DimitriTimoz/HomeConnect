using System;
using System.Collections.Generic;
using System.Text;

namespace HomeConnect.Models
{
    public class GetTemps
    {
        public string id { get; set; }
        public string date { get; set; }
        public int current { get; set; }
        public int average { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public int toID { get; set; }
    }


}
