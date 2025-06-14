using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMulti.Constants
{
    public static class AppConstants
    {
        public  static Dictionary<string, string> ThemesDict = new Dictionary<string, string> {
                        {"eb4.json",     "Эл. безопасность 4 гр. 2025"},
                        {"eb5.json",     "Эл. безопасность 5 гр. 2025"},
                        {"ot.json",      "Охрана труда 2025"},
                        {"vis.json",     "Работы на высоте 2025"}
        };
        public const int MAX_TIME_FOR_ANSWER = 15; // сек, после вопрос отправлятеся в долгие ответы
    }
}
