using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TestMulti.Constants;

namespace TestMulti.Models
{
    
    public partial class Quest : ObservableObject
    {      
        public string Theme { get; set; }
        public string FileName { get; set; }
        public string number { get; set; }
        public string title { get; set; }

        public ObservableCollection<Answer> answers { get; set; }
        public string DisplayTitle => $"{number}. {title}";

        [ObservableProperty]
        public bool vaworites = false; // Избранный вопрос       


        public  bool firstAttempt = true;

        public string QuestColor { get; set; } = "Gray"; // Серый цвет по умолчанию
        
        public double Ellapsed { get; set; } = 0; // Время, прошедшее с момента начала вопроса

        public static Quest [] GetForLearn(Quest[] quests)
        {
            return quests
                .Where(q => q.QuestColor == "Red" || q.QuestColor == "Gray" || q.Ellapsed >= AppConstants.MAX_TIME_FOR_ANSWER)
                .ToArray();
        }

        public static int GetLearnCount(Quest[] quests)
        {
            int learnCount = 0;
            foreach (var quest in quests)
            {
                if (quest.QuestColor != "Gray")
                {
                    learnCount++;
                }
            }
            return learnCount;
        }

        public static int GetErrorsCount(Quest[] quests)
        {
            int errorsCount = 0;
            foreach (var quest in quests)
            {
                if (quest.QuestColor == "Red")
                {
                    errorsCount++;
                }
            }
            return errorsCount;
        }
        public static int GetCorrectCount(Quest[] quests)
        {
            int correctCount = 0;
            foreach (var quest in quests)
            {
                if (quest.QuestColor == "Green")
                {
                    correctCount++;
                }
            }
            return correctCount;
        }

        public static int GetVaworiteCount(Quest[] quests)
        {
            int count = 0;
            foreach (var quest in quests)
            {
                if (quest.Vaworites)
                {
                    count++;
                }
            }
            return count;
        }

        public static int GetLongCount(Quest[] quests)
        {
            int longCount = 0;
            foreach (var quest in quests)
            {
                if (quest.Ellapsed > AppConstants.MAX_TIME_FOR_ANSWER)
                {
                    longCount++;
                }
            }
            return longCount;
        }

    }
    
}



