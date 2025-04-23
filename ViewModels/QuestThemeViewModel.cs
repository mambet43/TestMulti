using TestMulti.Models;
using TestMulti.Services;

namespace TestMulti.ViewModels;

[QueryProperty(nameof(Theme), "theme")]
public class QuestThemeViewModel : ContentView
{
    public  string Theme { get; set; }
    public Quest [] Quest { get; set; } 

    public QuestThemeViewModel()
	{
		Quest = JsonManager.DeserializeFromJson(Theme + ".json");
    }
}