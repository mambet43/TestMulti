
using TestMulti.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using TestMulti.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TestMulti.Constants;

namespace TestMulti.ViewModels;

public partial class MainPage : ObservableObject, INotifyPropertyChanged
{

    [ObservableProperty]
    private ObservableCollection<Theme> themes = new ObservableCollection<Theme>();

    public static MainPage Instance { get; private set; }

    private readonly ContentPage mainPage;


    public MainPage(ContentPage page)
    {       
        LoadQuest();
        Instance = this;
        mainPage = page;        
        Routing.RegisterRoute("QuestionPage", typeof(QuestionPage));
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