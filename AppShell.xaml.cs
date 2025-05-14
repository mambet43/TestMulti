using TestMulti.Services;
using TestMulti.Views;
using TestMulti.ViewModels;
using TestMulti.Models;

namespace TestMulti
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
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
                JsonManager.EditPreferences(Theme.CurrentQuests);
                ViewModels.MainPage.Instance.LoadQuest();
            }
        }
    }
}
