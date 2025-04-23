namespace TestMulti.Models
{
    
    public class Quest
    {

        public string number { get; set; }
        public string title { get; set; }
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
   

