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

    public QuestThemeViewModel(string theme)
	{
        switch (theme)
        {
            case "eb":          Theme = "Электробезопасность";  break;
            case "ot":          Theme = "Охрана труда";         break;
            case "vis":         Theme = "Работы на высоте";     break;
            case "vaworites":   Theme = "Избранные вопросы";
                                JsonManager.VaworitesCreate();
                                break;
            default:            Theme = "Error";                break ;
        }
        FileName = theme + ".json";
        Quest = JsonManager.DeserializeFromJson(FileName);
        ViewQuestTheme();
    }

    [RelayCommand]
    private void SetVaworites(Quest quest)
    {
        if (quest != null)
        {
            if (Theme != "Избранные вопросы") quest.Theme = Theme;
            quest.Vaworites = !quest.Vaworites;
        }
        if (Theme == "Избранные вопросы") JsonManager.EditPreferences(quest);
        else JsonManager.UserQuestSave(Quest, FileName);        
        var mainPage = MainPage.Instance;
        mainPage.LoadQuest();
    }

    private void ViewQuestTheme()
    {
        foreach (var quest in Quest)
        {
            QuestCollection.Add(quest);            
        }

    }    
}