using CommunityToolkit.Maui.Views;
using TestMulti.Constants;
using TestMulti.Models;
using TestMulti.Services;
using TestMulti.ViewModels;
using TestMulti.Views;


namespace TestMulti
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            foreach (KeyValuePair<string, string> kvp in AppConstants.THEMES)
            {
                var menuItem = new MenuItem
                {
                    Text = kvp.Value,
                    Command = new Command<string>(param => MenuItemCommand(param)),
                    CommandParameter = kvp.Key
                };                
                
                Items.Add(menuItem);
            }
            var menuItemVaworites = new MenuItem
            {
                Text = "Избранные вопросы",
                Command = new Command<string>(param => MenuItemCommand(param)),
                CommandParameter = "vaworites"
            };

            Items.Add(menuItemVaworites);


            Routing.RegisterRoute("QuestTheme", typeof(QuestTheme));
        }
        private async void MenuItemCommand(string file)
        {
            // Передаем параметр "value1" при навигации
            await Shell.Current.GoToAsync("QuestTheme?file="+file, false);
            Shell.Current.FlyoutIsPresented = false;
        }
        

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            if (args.Source == ShellNavigationSource.PopToRoot)
            {
                var popup = new LoadingPopup();
                this.ShowPopup(popup); // Показываем Popup, но НЕ ждем его закрытия

                await Task.Run(async () => await JsonManager.EditPreferences(Theme.CurrentQuests));
                Theme.QuestsVaworiteAll.Clear();
                popup.Close(); // Закрываем Popup после завершения задачи

                ViewModels.MainPage.Instance.LoadQuest();
            }
        }


    }
}
