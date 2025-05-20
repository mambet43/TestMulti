using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMulti.Constants
{
    public static class AppConstants
    {
        // для добавления темы тестирования необходимо добавить сформированный json в папку ресурсов и установить свойство внедреный ресурс
        public readonly static Dictionary<string, string> THEMES = new Dictionary<string, string> {
                        {"eb4.json",     "Электробезопасность 4 гр."},
                        {"eb5.json",     "Электробезопасность 5 гр."},
                        {"ot.json",      "Охрана труда"},
                        {"vis.json",     "Работы на высоте"},
                        {"pb.json",      "Пожарная безопасность"}
                        };
        public const int MAX_TIME_FOR_ANSWER = 2;
    }
}
