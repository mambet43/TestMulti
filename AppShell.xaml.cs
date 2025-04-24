using TestMulti.Views;

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
            await Shell.Current.GoToAsync("QuestTheme?param=eb");
            Shell.Current.FlyoutIsPresented = false;
        }
        private async void OnMenuItemClickedOt(object sender, EventArgs e)
        {
            // Передаем параметр "value1" при навигации
            await Shell.Current.GoToAsync("QuestTheme?param=ot");
            Shell.Current.FlyoutIsPresented = false;
        }
        private async void OnMenuItemClickedVis(object sender, EventArgs e)
        {
            // Передаем параметр "value1" при навигации
            await Shell.Current.GoToAsync("QuestTheme?param=vis");
            Shell.Current.FlyoutIsPresented = false;
        }
        private async void OnMenuItemClickedVaworites(object sender, EventArgs e)
        {
            // Передаем параметр "value1" при навигации
            await Shell.Current.GoToAsync("QuestTheme?param=vaworites");
            Shell.Current.FlyoutIsPresented = false;
        }
    }
}
