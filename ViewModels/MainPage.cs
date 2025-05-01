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

namespace TestMulti.ViewModels;

public partial class MainPage : ContentView
{
    private readonly HashSet<ChartPoint> _activePoints = [];
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


    public IEnumerable<ISeries> Series { get; set; } =
        GaugeGenerator.BuildSolidGauge(            
            new GaugeItem(30, series => SetStyle("Ошибок", series, SKColors.Red)),
            new GaugeItem(10, series => SetStyle("Медленных", series, SKColors.Yellow)),
            new GaugeItem(70, series => SetStyle("Верных", series, SKColors.Green)),
            new GaugeItem(100, series => SetStyle("Вопросов в теме", series, SKColors.Blue)),
            new GaugeItem(GaugeItem.Background, series =>
            {
                series.InnerRadius = 10;
            }));

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

    

    public MainPage()
	{
        Shell.Current.Navigating += OnNavigating;
    }
    private void OnNavigating(object sender, ShellNavigatingEventArgs e)
    {
        
    }
}