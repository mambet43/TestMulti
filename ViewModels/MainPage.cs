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

namespace TestMulti.ViewModels;

public partial class MainPage : ObservableObject
{
    [ObservableProperty]
    private string lengthEb;
    [ObservableProperty]
    private string lengthOt;

    [ObservableProperty]
    private string lengthVis;

    [ObservableProperty]
    private string lengthEbReplay;

    [ObservableProperty]
    private string lengthOtReplay;

    [ObservableProperty]
    private string lengthVisReplay;

    [ObservableProperty]
    private string errEbCount;

    [ObservableProperty]
    private string errOtCount;

    [ObservableProperty]
    private string errVisCount;

    [ObservableProperty]
    private string vaworiteEbCount;

    [ObservableProperty]
    private string vaworiteOtCount;

    [ObservableProperty]
    private string vaworiteVisCount;

    [ObservableProperty]
    private string longEbCount;

    [ObservableProperty]
    private string longOtCount;

    [ObservableProperty]
    private string longVisCount;

    private Quest[] QuestsEb;
    private Quest[] QuestsOt;
    private Quest[] QuestsVis;

    public IEnumerable<ISeries> SeriesEb { get; set; }
    public IEnumerable<ISeries> SeriesOt { get; set; } 
    public IEnumerable<ISeries> SeriesVis { get; set; }

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

    public void LoadQuest()
    {
        QuestsEb = JsonManager.DeserializeFromJson("eb.json");
        QuestsOt = JsonManager.DeserializeFromJson("ot.json");
        QuestsVis = JsonManager.DeserializeFromJson("vis.json");

        LengthEb = QuestsEb.Length.ToString();
        LengthOt = QuestsOt.Length.ToString();
        LengthVis = QuestsVis.Length.ToString(); 

        LengthEbReplay = Quest.GetForLearn(QuestsEb).Length.ToString();
        LengthOtReplay = Quest.GetForLearn(QuestsOt).Length.ToString();
        LengthVisReplay = Quest.GetForLearn(QuestsVis).Length.ToString();

        ErrEbCount = Quest.GetErrorsCount(QuestsEb).ToString();
        ErrOtCount = Quest.GetErrorsCount(QuestsOt).ToString();
        ErrVisCount = Quest.GetErrorsCount(QuestsVis).ToString();

        VaworiteEbCount = Quest.GetVaworiteCount(QuestsEb).ToString();
        VaworiteOtCount = Quest.GetVaworiteCount(QuestsOt).ToString();
        VaworiteVisCount = Quest.GetVaworiteCount(QuestsVis).ToString();
       

        LongEbCount = Quest.GetLongCount(QuestsEb).ToString();
        LongOtCount = Quest.GetLongCount(QuestsOt).ToString();
        LongVisCount = Quest.GetLongCount(QuestsVis).ToString();
        
        SeriesEb = GaugeGenerator.BuildSolidGauge(
            new GaugeItem(Quest.GetErrorsCount(QuestsEb), seriesEb => SetStyle("Ошибок", seriesEb, SKColors.Red)),
            new GaugeItem(Quest.GetLongCount(QuestsEb), seriesEb => SetStyle("Долгих ответов", seriesEb, SKColors.Yellow)),
            new GaugeItem(Quest.GetCorrectCount(QuestsEb), seriesEb => SetStyle("Верных", seriesEb, SKColors.Green)),
            new GaugeItem(Quest.GetLearnCount(QuestsEb), seriesEb => SetStyle("Пройдено", seriesEb, SKColors.Blue)),
            new GaugeItem(QuestsEb.Length, seriesEb => SetStyle("Вопросов в теме", seriesEb, SKColors.Blue)),
            new GaugeItem(GaugeItem.Background, seriesEb =>
            {
                seriesEb.InnerRadius = 10;
            }));

        SeriesOt = GaugeGenerator.BuildSolidGauge(
            new GaugeItem(Quest.GetErrorsCount(QuestsOt), seriesOt => SetStyle("Ошибок", seriesOt, SKColors.Red)),
            new GaugeItem(Quest.GetLongCount(QuestsOt), seriesOt => SetStyle("Долгих ответов", seriesOt, SKColors.Yellow)),
            new GaugeItem(Quest.GetCorrectCount(QuestsOt), seriesOt => SetStyle("Верных", seriesOt, SKColors.Green)),
            new GaugeItem(Quest.GetLearnCount(QuestsOt), seriesOt => SetStyle("Пройдено", seriesOt, SKColors.Blue)),
            new GaugeItem(QuestsOt.Length, seriesOt => SetStyle("Вопросов в теме", seriesOt, SKColors.Blue)),
            new GaugeItem(GaugeItem.Background, seriesOt =>
            {
                seriesOt.InnerRadius = 10;
            }));

        SeriesVis = GaugeGenerator.BuildSolidGauge(
            new GaugeItem(Quest.GetErrorsCount(QuestsVis), seriesVis => SetStyle("Ошибок", seriesVis, SKColors.Red)),
            new GaugeItem(Quest.GetLongCount(QuestsVis), seriesVis => SetStyle("Долгих ответов", seriesVis, SKColors.Yellow)),
            new GaugeItem(Quest.GetCorrectCount(QuestsVis), seriesVis => SetStyle("Верных", seriesVis, SKColors.Green)),
            new GaugeItem(Quest.GetLearnCount(QuestsVis), seriesVis => SetStyle("Пройдено", seriesVis, SKColors.Blue)),
            new GaugeItem(QuestsVis.Length, seriesVis => SetStyle("Вопросов в теме", seriesVis, SKColors.Blue)),
            new GaugeItem(GaugeItem.Background, seriesVis =>
            {
                seriesVis.InnerRadius = 10;
            }));

    }


    

   
}