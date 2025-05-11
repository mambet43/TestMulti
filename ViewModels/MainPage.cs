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




    private async Task<ObservableCollection<Theme>> LoadThemeList()
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
                           q.Ellapsed >= AppConstants.MAX_TIME_FOR_ANSWER).ToObservableCollection();
            theme.QuestsErr = theme.Quests.Where(q => (q.QuestColor == "Red")).ToObservableCollection();
            theme.QuestsVaworite = theme.Quests.Where(q => (q.vaworites)).ToObservableCollection();
            theme.QuestsLong = theme.Quests.Where(q => (q.Ellapsed >= AppConstants.MAX_TIME_FOR_ANSWER)).ToObservableCollection();
            theme.QuestsCorrect = theme.Quests.Where(q => (q.QuestColor == "Green")).ToObservableCollection();
            theme.QuestsLearn = theme.Quests.Where(q => (q.QuestColor != "Gray")).ToObservableCollection();
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