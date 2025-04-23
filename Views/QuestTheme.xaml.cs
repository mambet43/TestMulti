using TestMulti.ViewModels;

namespace TestMulti.Views;

public partial class QuestTheme : ContentPage
{
	public QuestTheme()
	{
		InitializeComponent();
        BindingContext = new QuestThemeViewModel();
    }
}