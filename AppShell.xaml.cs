using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Storage;
using System.Reflection;
using TestMulti.Constants;
using TestMulti.Models;
using TestMulti.Services;
using TestMulti.ViewModels;
using TestMulti.Views;


namespace TestMulti
{
    public partial class AppShell : Shell
    {
        private bool CurrentPageIsMain()
        {
            return Shell.Current.CurrentPage is MainPage;
        }

        public AppShell()
        {
            InitializeComponent(); 
            foreach (KeyValuePair<string, string> kvp in AppConstants.THEMES)
            {
                var menuItem = new MenuItem
                {
                    Text = kvp.Value,
                    Command = new Command<string>(param => MenuItemCommand(param)),
                    CommandParameter = kvp.Key,
                    IconImageSource = ImageSource.FromResource("TestMulti.Resources.Images.lib.png")
                };                
                
                Items.Add(menuItem);
            }
            var menuItemVaworites = new MenuItem
            {
                Text = "Избранные вопросы",
                IconImageSource = ImageSource.FromResource("TestMulti.Resources.Images.star.png"),
                Command = new Command<string>(param => MenuItemCommand(param)),
                CommandParameter = "vaworites"
            };

            Items.Add(menuItemVaworites);


            Routing.RegisterRoute("QuestTheme", typeof(QuestTheme));
        }
        private async void MenuItemCommand(string file)
        {         
            await Shell.Current.GoToAsync("QuestTheme?file="+file, false);
            Shell.Current.FlyoutIsPresented = false;
        }
        

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);
            if (args.Source == ShellNavigationSource.PopToRoot && Theme.IsChange)
            {
                bool result = await MainPage.Instance.DisplayAlert(
                           "Сохранение",
                           "Сохранить результаты?",
                           "Да",
                           "Нет");
                if (result)
                {
                    var popup = new LoadingPopup();
                    this.ShowPopup(popup); // Показываем Popup, но НЕ ждем его закрытия
                    await Task.Run(async () => await JsonManager.EditPreferences(Theme.CurrentQuests));
                    Theme.QuestsVaworiteAll.Clear();
                    popup.Close(); // Закрываем Popup после завершения задачи
                    await ViewModels.MainPage.Instance.RefreshAsync();
                }     
            }            
        }


    }
}
