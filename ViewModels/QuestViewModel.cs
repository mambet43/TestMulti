namespace TestMulti.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TestMulti.Models;
using TestMulti.Services;

public partial class QuestViewModel : QuestThemeViewModel
{
    [ObservableProperty]
    private int currentPosition;


    public QuestViewModel(string theme):base(theme)
    {
        CurrentPosition = 0;
    }

    [RelayCommand]
    private void NextQuestion()
    {
        if (CurrentPosition < QuestCollection.Count - 1)
        {
            CurrentPosition++;
        }
    }

    [RelayCommand]
    private void PreviousQuestion()
    {
        if (CurrentPosition > 0)
        {
            CurrentPosition--;
        }
    }


    [RelayCommand]
    private void AnswerSelected(Answer selectedAnswer)
    {
        var mainPage = MainPage.Instance;
        if (selectedAnswer == null) return;
       
        foreach (Answer answer in base.Quest[currentPosition-1].answers)
        {
            if (answer == selectedAnswer)
            {
                if (selectedAnswer.correct)
                {
                    answer.BackgroundColorHex = "#5F9EA0";  // почти «еленый 
                    if (base.Quest[currentPosition - 1].QuestColor != "Red") base.Quest[currentPosition - 1].QuestColor = "Green";
                    JsonManager.EditPreferences(base.Quest[currentPosition - 1]);
                    CurrentPosition++;

                }
                else
                {
                    answer.BackgroundColorHex = "#D69D82"; // почти красный
                    JsonManager.EditPreferences(base.Quest[currentPosition - 1]);
                    base.Quest[currentPosition - 1].QuestColor = "Red";
                }
            }
            else
            {
                answer.BackgroundColorHex = "#00FFFFFF"; // —брасываем фон дл€ остальных (прозрачный)
            }
        }
        JsonManager.EditPreferences(base.Quest[currentPosition - 1]);        
        mainPage.LoadQuest();

    }

}