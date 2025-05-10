using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMulti.Constants;
using TestMulti.Services;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Events;
using System.Collections.ObjectModel;

namespace TestMulti.Models
{
    public class Theme
    {
        public string Title { get; set; }
        public string FileName { get; set; }
        public List<Quest> Quests { get; set; }

        public List<Quest> QuestsForReplay { get; set; }
        public List<Quest> QuestsErr { get; set; }
        public List<Quest> QuestsVaworite { get; set; }
        public List<Quest> QuestsLong { get; set; }
        public List<Quest> QuestsCorrect { get; set; }
        public List<Quest> QuestsLearn { get; set; }

        public int LengthQ { get; set; }
        public int LengthReplay => QuestsForReplay.Count;
        public int LengthErr => QuestsErr.Count;
        public int LengthVaworite => QuestsVaworite.Count;
        public int LengthLong => QuestsLong.Count;
        public int LengthCorrect => QuestsCorrect.Count;
        public int LengthLearn => QuestsLearn.Count;

        public ObservableCollection<ISeries> Series { get; set; } 



        public Theme()
        {           
           
        }
    }

}
