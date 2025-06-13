using TestMulti.Models;
using TestMulti.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using TestMulti.Extentions;

namespace TestMulti.ViewModels;


public partial class QuestThemeViewModel : ObservableObject
{

    public ObservableCollection<Quest> QuestCollection { get; } = new ObservableCollection<Quest>();

    [ObservableProperty]
    private string titlePage;

    public QuestThemeViewModel(string file)
	{
        string decodedFile = Uri.UnescapeDataString(file);

        if (decodedFile == "vaworites")
        {
            foreach (var theme in MainPage.Instance.Themes)
            {
                if (theme.QuestsVaworite != null && theme.QuestsVaworite.Count > 0)
                {
                    Theme.QuestsVaworiteAll.AddRange(theme.QuestsVaworite);
                }
            }
            Theme.QuestsVaworiteAll = new ObservableCollection<Quest>(
                Theme.QuestsVaworiteAll
                    .GroupBy(q => q.title)
                    .Select(g => g.First())
                    .ToList()
            );
            Theme.CurrentQuests = Theme.QuestsVaworiteAll;
            TitlePage = "Избранные вопросы";
        }    
           
        else
        {
            foreach (var item in MainPage.Instance.Themes)
            {
                if (item.FileName == decodedFile) Theme.CurrentQuests = item.Quests;
            }
            TitlePage = Theme.CurrentQuests != null && Theme.CurrentQuests.Count > 0 ? Theme.CurrentQuests[0].Theme : "Нет вопросов";
        }        
        QuestCollection = Theme.CurrentQuests;

    }


    public QuestThemeViewModel()
    {


    }
    [RelayCommand]
    private void SetVaworites(Quest quest)
    {       
        if (quest != null)
        {           
            quest.Vaworites = !quest.Vaworites;
        }
        Theme.IsChange = true;
        //JsonManager.EditPreferences(quest);          
        
    }

 
}