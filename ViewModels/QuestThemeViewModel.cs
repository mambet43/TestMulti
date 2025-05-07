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
    
    public Quest [] Quest { get; set; }

    [ObservableProperty]
    public string Theme { get; set; }

    public string FileName { get; set; }

    private async void LoadQuest(string theme)
    {
        Quest = await JsonManager.DeserializeFromJson(FileName);
    }

    public QuestThemeViewModel(string theme)
	{
        switch (theme)  
        {
            case "eb":          Theme = "Электробезопасность";  break;
            case "ot":          Theme = "Охрана труда";         break;
            case "vis":         Theme = "Работы на высоте";     break;
            case "vaworites":   Theme = "Избранные вопросы";    break;
        }
        FileName = theme + ".json";
        LoadQuest(FileName);
        foreach (var quest in Quest)
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
        var mainPage = MainPage.Instance;
        mainPage.LoadQuest();
    }

 
}