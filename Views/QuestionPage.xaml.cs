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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdatePageAppearance();
    }


    private void UpdatePageAppearance()
    {
        BindingContext = new ViewModels.QuestViewModel(PageTheme);
    }


    public QuestionPage()
	{
		InitializeComponent();

    }
}