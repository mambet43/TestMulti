using TestMulti.Models;
using TestMulti.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;

namespace TestMulti.ViewModels;


public partial class QuestThemeViewModel : ObservableObject
{
    public ObservableCollection<Quest> QuestCollection { get; } = new ObservableCollection<Quest>();
     

    public QuestThemeViewModel()
	{
        foreach (var quest in Theme.CurrentQuests)
        {
            QuestCollection.Add(quest);
        }
    }

    [RelayCommand]
    private void SetVaworites(Quest quest)
    {       
        if (quest != null)
        {           
            quest.Vaworites = !quest.Vaworites;
        }
        JsonManager.EditPreferences(quest);          
        
    }

 
}