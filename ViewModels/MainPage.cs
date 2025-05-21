
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using TestMulti.Constants;
using TestMulti.Models;
using TestMulti.Views;

namespace TestMulti.ViewModels;

public partial class MainPage : ObservableObject, INotifyPropertyChanged
{

    [ObservableProperty]
    private ObservableCollection<Theme> themes = new ObservableCollection<Theme>();


    [ObservableProperty]
    private int currentPosition;
    public static MainPage Instance { get; private set; }

    private readonly ContentPage mainPage;
    

    public MainPage(ContentPage page)
    {       
        LoadQuest();        
        Instance = this;
        mainPage = page;        
        Routing.RegisterRoute("QuestionPage", typeof(QuestionPage));
    }

    [ObservableProperty]
    private bool isRefreshing;

    [RelayCommand]
    public async Task RefreshAsync()
    {
        int position = CurrentPosition;
        IsRefreshing = true;        
        LoadQuest();
        await Task.Delay(500);
        CurrentPosition = position;       
        IsRefreshing = false;
    }


    [RelayCommand]
    private void CurrentItemChanged()
    {  
       

    }



    public  void LoadQuest()
    {
        themes.Clear();
        foreach (KeyValuePair<string, string> kvp in AppConstants.THEMES)
        {            
            Theme theme = new Theme(kvp.Key, kvp.Value);
            themes.Add(theme);
        }
           
    }
    




}