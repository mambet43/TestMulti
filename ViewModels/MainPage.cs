namespace TestMulti.ViewModels;

public class MainPage : ContentView
{
	public MainPage()
	{
        Shell.Current.Navigating += OnNavigating;
    }
    private void OnNavigating(object sender, ShellNavigatingEventArgs e)
    {
        
    }
}