using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatsUpApp.Models
{
    public class News
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public String Date { get; set; }
        public String Category { get; set; }
        public String Text { get; set; }
        public String Img { get; set; }
    }
}