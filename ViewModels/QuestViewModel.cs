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

    

    public QuestViewModel()
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
        if (selectedAnswer == null) return;

        var currentQuest = base.QuestCollection[currentPosition];
        bool isQuestUpdated = false;

        foreach (Answer answer in currentQuest.answers)
        {
            if (answer == selectedAnswer)
            {
                answer.BackgroundColorHex = selectedAnswer.correct ? "#5F9EA0" : "#D69D82";
                currentQuest.QuestColor = selectedAnswer.correct && currentQuest.QuestColor != "Red" ? "Green" : "Red";
                isQuestUpdated = true;
            }
            else
            {
                answer.BackgroundColorHex = "#00FFFFFF"; // Прозрачный
            }
        }

        if (isQuestUpdated)
        {           
            if (selectedAnswer.correct && CurrentPosition < QuestCollection.Count - 1)
            {
                CurrentPosition++; 
            }
        }

    }

}