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
               
        public int LengthQ { get; set; }
        public int LengthReplay => QuestsForReplay.Count;
        public int LengthErr => QuestsErr.Count;
        public int LengthVaworite => QuestsVaworite.Count;
        public int LengthLong => QuestsLong.Count;
        public int LengthCorrect => QuestsCorrect.Count;
        public int LengthLearn => QuestsLearn.Count;

        private MainPage mainPage = MainPage.Instance;

        public ObservableCollection<ISeries> Series { get; set; }


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
