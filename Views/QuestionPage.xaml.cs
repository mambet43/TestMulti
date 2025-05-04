namespace TestMulti.Views;

[QueryProperty(nameof(PageTheme), "theme")]

public partial class QuestionPage : ContentPage
{
    private string _pageTheme;
    public string PageTheme
    {
        get => _pageTheme;
        set
        {
            _pageTheme = value;
        }
    }

    public QuestionPage()
	{
		InitializeComponent();
		BindingContext = new ViewModels.QuestViewModel(PageTheme);
    }
}