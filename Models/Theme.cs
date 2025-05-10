using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMulti.Constants;
using TestMulti.Services;

namespace TestMulti.Models
{
    internal class Theme
    {
        public string Title { get; set; }
        public string FileName { get; set; }
        public List<Quest> Quests { get; set; }

        public List<Quest> QuestsForReplay { get; set; }
        public List<Quest> QuestsErr { get; set; }
        public List<Quest> QuestsVaworite { get; set; }
        public List<Quest> QuestsLong { get; set; }
        public int Length => Quests.Count;
        public int LengthReplay => QuestsForReplay.Count;
        public int LengthErr => QuestsErr.Count;
        public int LengthVaworite => QuestsVaworite.Count;
        public int LengthLong => QuestsLong.Count;


        public Theme()
        {           
           
        }
    }

}
