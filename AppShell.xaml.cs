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
                string filename = kvp.Key;
                MenuItem menuItem = new MenuItem();
                menuItem.Text = kvp.Value;
                Items.Add(menuItem);
            }


            Routing.RegisterRoute("QuestTheme", typeof(QuestTheme));
        }
        private async void OnMenuItemClickedEb(object sender, EventArgs e)
        {
            // Передаем параметр "value1" при навигации
            await Shell.Current.GoToAsync("QuestTheme?param=eb", false);
            Shell.Current.FlyoutIsPresented = false;
        }
        private async void OnMenuItemClickedOt(object sender, EventArgs e)
        {
            // Передаем параметр "value1" при навигации
            await Shell.Current.GoToAsync("QuestTheme?param=ot", false);
            Shell.Current.FlyoutIsPresented = false;
        }
        private async void OnMenuItemClickedVis(object sender, EventArgs e)
        {
            // Передаем параметр "value1" при навигации
            await Shell.Current.GoToAsync("QuestTheme?param=vis", false);
            Shell.Current.FlyoutIsPresented = false;
        }
        private async void OnMenuItemClickedVaworites(object sender, EventArgs e)
        {
            // Передаем параметр "value1" при навигации
            await Shell.Current.GoToAsync("QuestTheme?param=vaworites", false);
            Shell.Current.FlyoutIsPresented = false;
        }
        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            if (args.Source == ShellNavigationSource.PopToRoot)
            {
                var popup = new LoadingPopup();
                this.ShowPopup(popup);
                await Task.Delay(5000);
                await JsonManager.EditPreferences(Theme.CurrentQuests);
                popup.Close(); 
                ViewModels.MainPage.Instance.LoadQuest();
            }
        }
    }
}
