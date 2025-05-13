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
using TestMulti.Extentions;

namespace TestMulti.Models
{
    public partial class Theme : ObservableObject
    {
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

        

        public Theme()
        { 
            
           
        }
    }

}
