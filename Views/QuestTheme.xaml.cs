using CommunityToolkit.Mvvm.ComponentModel;
using TestMulti.ViewModels;

namespace TestMulti.Views;


[QueryProperty(nameof(File), "file")]
public partial class QuestTheme : ContentPage
{
    public string File { get; set; }

   

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdatePageAppearance();
    }


    private void UpdatePageAppearance()
    {
        BindingContext = new QuestThemeViewModel(File);
    }
    public QuestTheme()
	{
		InitializeComponent();        
    }
   
}