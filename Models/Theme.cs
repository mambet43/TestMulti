using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMulti.Constants;
using TestMulti.Services;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Events;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using TestMulti.Views;
using LiveChartsCore.Themes;
using SkiaSharp;
using TestMulti.Extentions;
using Microsoft.Maui.Storage;

namespace TestMulti.Models
{
    public partial class Theme : ObservableObject
    {
        public static ObservableCollection<Theme> Themes { get; set; } = new ObservableCollection<Theme>();
        public string Title { get; set; }
        public string FileName { get; set; }
        public ObservableCollection<Quest> Quests { get; set; }
        public ObservableCollection<Quest> QuestsForReplay { get; set; }
        public ObservableCollection<Quest> QuestsErr { get; set; }
        public ObservableCollection<Quest> QuestsVaworite { get; set; }
        public ObservableCollection<Quest> QuestsLong { get; set; }
        public ObservableCollection<Quest> QuestsCorrect { get; set; }
        public ObservableCollection<Quest> QuestsLearn { get; set; }

        public static ObservableCollection<Quest> CurrentQuests { get; set; }
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
        private async void StartClicked()
        {
            CurrentQuests = Quests;
            await Shell.Current.GoToAsync("QuestionPage");
        }

        [RelayCommand]
        private async void ReplayClicked()
        {
            CurrentQuests = QuestsForReplay;
            await Shell.Current.GoToAsync("QuestionPage");
        }


        [RelayCommand]
        private async void MyErrorClicked()
        {
            CurrentQuests = QuestsErr;
            await Shell.Current.GoToAsync("QuestionPage");
        }

        [RelayCommand]
        private async void VaworiteClicked()
        {
            CurrentQuests = QuestsVaworite;
            await Shell.Current.GoToAsync("QuestionPage");
        }

        [RelayCommand]
        private async void LongClicked()
        {
            CurrentQuests = QuestsLong;
            await Shell.Current.GoToAsync("QuestionPage");
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

        public void LoaderThemes()
        {
            foreach (KeyValuePair<string, string> kvp in AppConstants.THEMES)
            {
                Theme theme = new Theme();
                theme.FileName = kvp.Key;
                theme.Title = kvp.Value;
                Task.Run(async () =>
                {
                    theme.Quests = await JsonManager.DeserializeToList(FileName);
                }).Wait();
                theme.Quests.ForEach(q => q.Theme = theme.Title);
                theme.QuestsForReplay = theme.Quests.Where
                        (q => (q.QuestColor == "Red" ||
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
                Themes.Add(this);
            }
        }


        public Theme()
        {
            LoaderThemes();
        }
    }

}
