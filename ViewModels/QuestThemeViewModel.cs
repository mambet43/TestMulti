using TestMulti.Models;
using TestMulti.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TestMulti.ViewModels;


public partial class QuestThemeViewModel : ContentView
{
    
    public Quest [] Quest { get; set; }

    [ObservableProperty]
    public string Theme { get; set; }

    public QuestThemeViewModel(string theme)
	{
        
        Theme = theme;
        Quest = JsonManager.DeserializeFromJson(Theme + ".json");
        
        
    }
    private void UpdatePageAppearance()
    {
        // Логика изменения страницы на основе параметра
        //if (Theme == "eb")
        //{
        //    testLbl.Text = "Значение 1";
        //}
        //else if (Theme == "ot")
        //{
        //    testLbl.Text = "Значение 2";
        //}
        //else if (Theme == "vis")
        //{
        //    testLbl.Text = "Значение 3";
        //}
        //else if (Theme == "vaworites")
        //{
        //    testLbl.Text = "Значение 4";
        //}
        //else testLbl.Text = "Не работает";
    }
}