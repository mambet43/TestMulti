using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Events;
using System.Diagnostics;
using LiveChartsCore.Kernel;
using TestMulti.Models;
using TestMulti.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;
using TestMulti.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TestMulti.Constants;
using TestMulti.Extentions;
using System.Text.Json;
using Microsoft.Maui.Storage;

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

    



    private async Task<ObservableCollection<Theme>> LoadThemeList()
    {
       
        return Themes=Theme.Themes;
    }


    public async void LoadQuest()
    {
        new Theme();
        Themes = await LoadThemeList();  
    }
    




}