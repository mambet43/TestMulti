using CommunityToolkit.Mvvm.ComponentModel;
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

        public Answer[] answers { get; set; }
        public string DisplayTitle => $"{number}. {title}";

        [ObservableProperty]
        public bool vaworites = false; // Избранный вопрос       

        
       

        public string QuestColor { get; set; } = "Gray"; // Серый цвет по умолчанию
        public string AnswerColor { get; set; } = "Gray";  // Серый цвет по умолчанию
        public double Ellapsed { get; set; } = 0; // Время, прошедшее с момента начала вопроса

        public static Quest [] GetForLearn(Quest[] quests)
        {
            int length = 0;
            foreach (var quest in quests)
            {
                if ((quest.QuestColor == "Red" || quest.QuestColor == "Gray"  ) || quest.Ellapsed >= AppConstants.MAX_TIME_FOR_ANSWER) length++;                
            }
            Quest[] questsRet = new Quest[length];
            for (int i = 0; i < questsRet.Length; i++)
            {
                for (int j = 0; j < quests.Length; j++)
                {
                    questsRet[i] = quests[j];
                }
            }
            return questsRet;
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



