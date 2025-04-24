namespace TestMulti

{
    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.MainPage();
        }

        private void clearbtn_Clicked(object sender, EventArgs e)
        {
            Preferences.Clear();

        }
    }

}
