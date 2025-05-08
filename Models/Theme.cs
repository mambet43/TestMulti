using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMulti.Models
{
    internal class Theme
    {
        public string Title { get; set; }
        public string FileName { get; set; }
        public List <Quest> Quests { get; set; }
    }
}
