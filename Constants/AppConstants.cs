using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMulti.Constants
{
    public static class AppConstants
    {


        public  static Dictionary<string, string> ThemesDict = new Dictionary<string, string> {};
        public const int MAX_TIME_FOR_ANSWER = 15; // сек, после вопрос отправлятеся в долгие ответы
    }
}
