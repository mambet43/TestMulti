namespace TestMulti.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using TestMulti.Models;
using TestMulti.Services;

public partial class QuestViewModel : QuestThemeViewModel
{
    [ObservableProperty]
    private int currentPosition;
    private Stopwatch stopwatch = new Stopwatch();



    public QuestViewModel()
    {
        foreach (var quest in Theme.CurrentQuests)
        {
            QuestCollection.Add(quest);
        }
        base.TitlePage = QuestCollection != null && QuestCollection.Count > 0 ? QuestCollection[0].Theme : "Нет вопросов";
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
    private void CurrentItemChanged()
    {
        stopwatch.Reset();
        stopwatch.Start();
    }


    [RelayCommand]
    private void AnswerSelected(Answer selectedAnswer)
    {
        Theme.IsChange = true;
        if (selectedAnswer == null) return;

        var currentQuest = base.QuestCollection[currentPosition];
        bool isQuestUpdated = false;

        foreach (Answer answer in currentQuest.answers)
        {
            if (answer == selectedAnswer)
            {
                answer.BackgroundColorHex = selectedAnswer.correct ? "#5F9EA0" : "#D69D82";
                currentQuest.QuestColor = selectedAnswer.correct && (currentQuest.QuestColor != "Red" || Theme.IsReplay) ? "Green" : "Red";
                stopwatch.Stop();
                currentQuest.Ellapsed = Math.Max(currentQuest.Ellapsed, stopwatch.Elapsed.TotalSeconds);
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