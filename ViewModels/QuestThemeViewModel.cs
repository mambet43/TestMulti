using TestMulti.Models;
using TestMulti.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace TestMulti.ViewModels;


public partial class QuestThemeViewModel : ObservableObject
{
    public ObservableCollection<Quest> QuestCollection { get; } = new ObservableCollection<Quest>();
    
    public Quest [] Quest { get; set; }

    [ObservableProperty]

    public string Theme { get; set; }

    

    public QuestThemeViewModel(string theme)
	{
        switch (theme)
        {
            case "eb":          Theme = "Электробезопасность";  break;
            case "ot":          Theme = "Охрана труда";         break;
            case "vis":         Theme = "Работы на высоте";     break;
            case "vaworites":   Theme = "Избранные вопросы";    break;
            default:            Theme = "Error";                break ;
        }        
          
        
        Quest = JsonManager.DeserializeFromJson(theme + ".json");
        ViewQuestTheme();
    }

    [RelayCommand]
    private void SetVaworites(Quest quest)
    {
        if (quest != null)
        {
            quest.Vaworites = !quest.Vaworites;
        }
    }



    private void ViewQuestTheme()
    {
        foreach (var quest in Quest)
        {
            QuestCollection.Add(quest);            
        }

    }


    
}