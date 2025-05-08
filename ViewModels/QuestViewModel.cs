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
    private MainPage mainPage;

    public QuestViewModel(string theme):base(theme)
    {
        mainPage = MainPage.Instance;
  

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

        var currentQuest = base.Quest[currentPosition];
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
            //JsonManager.EditPreferences(currentQuest); // Обновляем только один раз
            if (selectedAnswer.correct && CurrentPosition < QuestCollection.Count - 1)
            {
                CurrentPosition++; // Переход к следующему вопросу
            }
        }
        //mainPage.LoadQuest();


    }

}