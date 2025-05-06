namespace TestMulti.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using TestMulti.Models;
using TestMulti.Services;

public partial class QuestViewModel : QuestThemeViewModel
{

    public QuestViewModel(string theme):base(theme)
    {
        
    }
 
}