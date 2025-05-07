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
       
        foreach (Answer answer in base.Quest[currentPosition].answers)
        {
            if (answer == selectedAnswer)
            {
                if (selectedAnswer.correct)
                {
                    answer.BackgroundColorHex = "#5F9EA0";  // почти «еленый 
                    if (base.Quest[currentPosition].QuestColor != "Red") base.Quest[currentPosition].QuestColor = "Green";
                    JsonManager.EditPreferences(base.Quest[currentPosition]);
                    CurrentPosition++;

                }
                else
                {
                    answer.BackgroundColorHex = "#D69D82"; // почти красный
                    JsonManager.EditPreferences(base.Quest[currentPosition]);
                    base.Quest[currentPosition].QuestColor = "Red";
                }
            }
            else
            {
                answer.BackgroundColorHex = "#00FFFFFF"; // —брасываем фон дл€ остальных (прозрачный)
            }
        }
        JsonManager.EditPreferences(base.Quest[currentPosition]);        
        mainPage.LoadQuest();

    }

}