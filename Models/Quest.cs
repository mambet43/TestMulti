using CommunityToolkit.Mvvm.ComponentModel;

namespace TestMulti.Models
{
    
    public partial class Quest : ObservableObject
    {

        public string number { get; set; }
        public string title { get; set; }
        public string DisplayTitle => $"{number}. {title}";
        [ObservableProperty]
        public bool vaworites = false; // Избранный вопрос
        public Answer[] answers { get; set; }
        public Color QwestColor { get; set; } = Colors.Gray; // Серый цвет по умолчанию
        public Color AnswerColor { get; set; } = Colors.Gray; // Серый цвет по умолчанию
        public int Ellapsed { get; set; } = 0; // Время, прошедшее с момента начала вопроса

    }
    public class Answer
    {
        public string title { get; set; }
        public bool correct { get; set; }
    }

}
   

