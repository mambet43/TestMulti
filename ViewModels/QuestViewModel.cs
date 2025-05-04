namespace TestMulti.ViewModels;

public class QuestViewModel : ContentView
{
    public string Theme { get; set; }


    public QuestViewModel(string theme)
	{
        Theme = theme;
    }
}