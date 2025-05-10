using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMulti.Constants
{
    public static class AppConstants
    {
        public readonly static Dictionary<string, string> THEMES = new Dictionary<string, string> {
                        { "eb.json", "Электробезопасность"},
                        {"ot.json", "Охрана труда"},
                        { "vis.json", "Охрана труда"} };
        public const int MAX_TIME_FOR_ANSWER = 15;
    }
}
