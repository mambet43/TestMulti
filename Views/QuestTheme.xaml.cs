using CommunityToolkit.Mvvm.ComponentModel;
using TestMulti.ViewModels;

namespace TestMulti.Views;

[QueryProperty(nameof(PageParam), "param")]


public partial class QuestTheme : ContentPage
{
    private string _pageParam;
    public string PageParam
    {
        get => _pageParam;
        set
        {
            _pageParam = value;
            OnPropertyChanged();
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdatePageAppearance();
    }


    private void UpdatePageAppearance()
    {
        BindingContext = new QuestThemeViewModel(PageParam);
    }
    public QuestTheme()
	{
		InitializeComponent();
        
    }
   
}