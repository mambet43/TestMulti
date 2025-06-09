using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.Themes;
using Microsoft.Maui.Storage;
using SkiaSharp;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestMulti.Constants;
using TestMulti.Extentions;
using TestMulti.Services;
using TestMulti.ViewModels;
using TestMulti.Views;

namespace TestMulti.Models
{
    public partial class Theme : ObservableObject
    {
        public string Title { get; set; }
        public LabelVisual PieTitle { get; set; }
        public string FileName { get; set; }
        public ObservableCollection<Quest> Quests { get; set; }
        public ObservableCollection<Quest> QuestsForReplay { get; set; }
        public ObservableCollection<Quest> QuestsErr { get; set; }
        public ObservableCollection<Quest> QuestsVaworite { get; set; }
        public ObservableCollection<Quest> QuestsLong { get; set; }
        public ObservableCollection<Quest> QuestsCorrect { get; set; }
        public ObservableCollection<Quest> QuestsLearn { get; set; }

        public static ObservableCollection<Quest> CurrentQuests { get; set; } = new ObservableCollection<Quest>();
        public static bool IsReplay {  get; set; }
        public static bool IsChange { get; set; }
        public static ObservableCollection<Quest> QuestsVaworiteAll { get; set; }  = new ObservableCollection<Quest>();
        [ObservableProperty]
        private int lengthQ;
        [ObservableProperty]
        private int lengthReplay;
        [ObservableProperty]
        private int lengthErr;
        [ObservableProperty]
        private int lengthVaworite;
        [ObservableProperty]
        private int lengthLong;
        [ObservableProperty]
        private int lengthCorrect;
        [ObservableProperty]
        private int lengthLearn; 

        private MainPage mainPage = MainPage.Instance;
        [ObservableProperty]
        private ObservableCollection<ISeries> series;


        [RelayCommand]
        private async Task ActionClicked(string action)
        {
            IsChange = false;
            IsReplay = false;
            switch (action)
            {
                case "Start":   
                    CurrentQuests = Quests;           
                    await Shell.Current.GoToAsync("QuestionPage");
                    break;

                case "Replay":
                    CurrentQuests = QuestsForReplay;
                    IsReplay = true;                   
                    await Shell.Current.GoToAsync("QuestionPage");
                    break;

                case "Vaworite":
                    CurrentQuests = QuestsVaworite;
                    await Shell.Current.GoToAsync("QuestionPage");
                    break;

                case "Error":
                    CurrentQuests = QuestsErr;       
                    await Shell.Current.GoToAsync("QuestTheme");
                    break;

                case "Long":
                    CurrentQuests = QuestsLong;
                    await Shell.Current.GoToAsync("QuestTheme");
                    break;

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
                ViewModels.MainPage.Instance.LoadQuest();
            }
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





        public Theme(string filename, string title)
        {
            FileName = filename;
            Title = title;
        }

        public static async Task<Theme> CreateAsync(string filename, string title)
        {
            var theme = new Theme(filename, title);
            theme.Quests = await JsonManager.DeserializeToList(filename);
            theme.lengthQ = theme.Quests.Count;
            theme.Quests.ForEach(q =>
            {
                q.Theme = title;
                q.FileName = filename;
                foreach (var a in q.answers)
                {
                    a.BackgroundColorHex = "#00FFFFFF";
                }
            });
            theme.QuestsForReplay = theme.Quests.Where(q => (q.QuestColor == "Red" || q.QuestColor == "Gray")
                || q.Ellapsed >= AppConstants.MAX_TIME_FOR_ANSWER).ToObservableCollection();
            theme.lengthReplay = theme.QuestsForReplay.Count;
            theme.QuestsErr = theme.Quests.Where(q => q.QuestColor == "Red").ToObservableCollection();
            theme.lengthErr = theme.QuestsErr.Count;
            theme.QuestsVaworite = theme.Quests.Where(q => q.vaworites).ToObservableCollection();
            QuestsVaworiteAll.AddRange(theme.QuestsVaworite);
            theme.lengthVaworite = theme.QuestsVaworite.Count;
            theme.QuestsLong = theme.Quests.Where(q => q.Ellapsed >= AppConstants.MAX_TIME_FOR_ANSWER).ToObservableCollection();
            theme.lengthLong = theme.QuestsLong.Count;
            theme.QuestsCorrect = theme.Quests.Where(q => q.QuestColor == "Green").ToObservableCollection();
            theme.lengthCorrect = theme.QuestsCorrect.Count;
            theme.QuestsLearn = theme.Quests.Where(q => q.QuestColor != "Gray").ToObservableCollection();
            theme.lengthLearn = theme.QuestsLearn.Count;
            theme.PieTitle = new LabelVisual
            {
                Text = title,
                TextSize = 20,
                Padding = new Padding(10),
                Paint = new SolidColorPaint(SKColors.White, 8)
            };
            theme.Series = new ObservableCollection<ISeries>(
                GaugeGenerator.BuildSolidGauge(
                    new GaugeItem(theme.lengthErr, series => SetStyle("Ошибок", series, SKColors.Red)),
                    new GaugeItem(theme.lengthLong, series => SetStyle("Долгих", series, SKColors.Yellow)),
                    new GaugeItem(theme.lengthCorrect, series => SetStyle("Верных", series, SKColors.Green)),
                    new GaugeItem(theme.lengthLearn, series => SetStyle("Пройдено", series, SKColors.Blue)),
                    new GaugeItem(theme.lengthQ, series => SetStyle("Вопросов", series, SKColors.Blue)),
                    new GaugeItem(GaugeItem.Background, series =>
                    {
                        series.InnerRadius = 10;
                    })
                )
            );
            Preferences.Set(filename, JsonSerializer.Serialize(theme.Quests));
            return theme;
        }
    }

}
