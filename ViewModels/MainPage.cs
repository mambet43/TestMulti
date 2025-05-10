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

namespace TestMulti.ViewModels;

public partial class MainPage : ObservableObject, INotifyPropertyChanged
{
    public ObservableCollection<Theme> ThemesCollection { get; } = new ObservableCollection<Theme>();

    
    public List<Theme> Themes { get; set; } = new List<Theme>();
    public static MainPage Instance { get; private set; }

    private readonly ContentPage mainPage;


    public MainPage(ContentPage page)
    {       
        LoadQuest();
        Instance = this;
        mainPage = page;
        Routing.RegisterRoute("QuestionPage", typeof(QuestionPage));
    }

    [RelayCommand]
    public void OnPressed(PointerCommandArgs args)
    {
        var foundPoints = args.Chart.GetPointsAt(args.PointerPosition);

        // Проверяем, что в коллекции достаточно элементов
        if (foundPoints.Count() >= 2)
        {
            var secondToLastPoint = foundPoints.ElementAt(foundPoints.Count() - 2);
            Debug.WriteLine($"Second to last point: {secondToLastPoint.Context.Series.Name}");
        }
        else
        {
            Debug.WriteLine("Not enough points to get the second to last element.");
        }
    }

    [RelayCommand]
    public async Task DelPreferences(string file)
    {
        bool result = await mainPage.DisplayAlert(
                        "Подтверждение",
                        "Вы точно хотите удалить все данные, чтобы начать заново?",
                        "Да",
                        "Отмена");
        if (result)
        {
            Preferences.Remove(file);
            LoadQuest();
        }
        
    }

    [RelayCommand]
    private async void StartClickedEb()
    {
        await Shell.Current.GoToAsync("QuestionPage?theme=eb");
    }
    [RelayCommand]
    private async void StartClickedOt()
    {
        await Shell.Current.GoToAsync("QuestionPage?theme=ot");
    }
    [RelayCommand]
    private async void StartClickedVis()
    {
        await Shell.Current.GoToAsync("QuestionPage?theme=vis");
    }



    [RelayCommand]
    private async void RetryClickedEb()
    {
        await Shell.Current.GoToAsync("QuestionPage?theme=eb");
    }

    private async Task <List<Theme>> LoadThemeList()
    {       
        foreach (KeyValuePair<string, string> kvp in AppConstants.THEMES)
        {
            Theme theme = new Theme();
            theme.FileName = kvp.Key;
            theme.Title = kvp.Value;
            theme.Quests = await JsonManager.DeserializeToList(theme.FileName);
            theme.Quests.ForEach(q => q.Theme += theme.Title);
            theme.QuestsForReplay = theme.Quests.Where 
                    (q => (q.QuestColor == "Red"   || 
                           q.QuestColor == "Gray") ||
                           q.Ellapsed >= AppConstants.MAX_TIME_FOR_ANSWER).ToList();
            theme.QuestsErr = theme.Quests.Where(q => (q.QuestColor == "Red")).ToList();
            theme.QuestsVaworite = theme.Quests.Where(q => (q.vaworites)).ToList();
            theme.QuestsLong = theme.Quests.Where(q => (q.Ellapsed >= AppConstants.MAX_TIME_FOR_ANSWER)).ToList();
            theme.QuestsCorrect = theme.Quests.Where(q => (q.QuestColor == "Green")).ToList();
            theme.QuestsLearn = theme.Quests.Where(q => (q.QuestColor != "Gray")).ToList();
            theme.LengthQ = theme.Quests.Count;
            theme.Series = new ObservableCollection<ISeries>(
            GaugeGenerator.BuildSolidGauge(
                new GaugeItem(theme.LengthErr, series => SetStyle("Ошибок", series, SKColors.Red)),
                new GaugeItem(theme.LengthLong, series => SetStyle("Долгих ответов", series, SKColors.Yellow)),
                new GaugeItem(theme.LengthCorrect, series => SetStyle("Верных", series, SKColors.Green)),
                new GaugeItem(theme.LengthLearn, series => SetStyle("Пройдено", series, SKColors.Blue)),
                new GaugeItem(theme.LengthQ, series => SetStyle("Вопросов в теме", series, SKColors.Blue)),
                new GaugeItem(GaugeItem.Background, series =>
                {
                    series.InnerRadius = 10;
                })));
            Themes.Add(theme);
            ThemesCollection.Add(theme);
        }
        return Themes;
    }
    public async void LoadQuest()
    {
        Themes = await LoadThemeList();        

        

    }
    public static void SetStyle(string name, PieSeries<ObservableValue> series, SKColor color)
    {
        series.Name = name;
        series.DataLabelsPosition = PolarLabelsPosition.Start;
        series.DataLabelsFormatter = point => $"{point.Coordinate.PrimaryValue} {point.Context.Series.Name}";
        series.DataLabelsPaint = new SolidColorPaint(SKColors.White);
        series.InnerRadius = 5;
        series.RelativeOuterRadius = 2;
        series.RelativeInnerRadius = 2;
        series.Fill = new SolidColorPaint(color);
    }




}