using CommunityToolkit.Mvvm.ComponentModel;
using TestMulti.ViewModels;

namespace TestMulti.Views;



public partial class QuestTheme : ContentPage
{
    

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdatePageAppearance();
    }


    private void UpdatePageAppearance()
    {
        BindingContext = new QuestThemeViewModel();
    }
    public QuestTheme()
	{
		InitializeComponent();        
    }
   
}