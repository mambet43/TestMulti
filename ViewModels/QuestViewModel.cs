namespace TestMulti.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using TestMulti.Models;
using TestMulti.Services;

public partial class QuestViewModel : ObservableObject
{
    public string Theme { get; set; }
    private Quest[] quests;

    [ObservableProperty]
    public string title;
    public ObservableCollection<Quest> QuestCollection { get; } = new ObservableCollection<Quest>();

    [RelayCommand]
    private void SetVaworites(Quest quest)
    {
        if (quest != null)
        {
            if (Theme != "Избранные вопросы") quest.Theme = Theme;
            quest.Vaworites = !quest.Vaworites;
        }
        if (Theme == "Избранные вопросы") JsonManager.EditPreferences(quest);
        else JsonManager.UserQuestSave(quests, Theme + ".json");
        var mainPage = MainPage.Instance;
        mainPage.LoadQuest();
    }

    public QuestViewModel(string theme)
	{
        Theme = theme;
        quests = JsonManager.DeserializeFromJson(Theme+".json");
        switch (theme)
        {
            case "eb": Title = "Электробезопасность"; break;
            case "ot": Title = "Охрана труда"; break;
            case "vis": Title = "Работы на высоте"; break;
            case "vaworites": Title = "Избранные вопросы"; break;
            default: Theme = "Error"; break;
        }
        foreach (var quest in quests)
        {
            QuestCollection.Add(quest);
        }
    }
}