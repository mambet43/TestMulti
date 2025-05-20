using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Activity;
using Microsoft.Maui.Controls;

namespace TestMulti
{
    [Activity(Theme = "@style/Maui.SplashTheme",
              MainLauncher = true,
              LaunchMode = LaunchMode.SingleTop,
              ConfigurationChanges = ConfigChanges.ScreenSize
                                   | ConfigChanges.Orientation
                                   | ConfigChanges.UiMode
                                   | ConfigChanges.ScreenLayout
                                   | ConfigChanges.SmallestScreenSize
                                   | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            OnBackPressedDispatcher.AddCallback(this, new CustomBackPressedCallback(true));
        }
    }

    public class CustomBackPressedCallback : OnBackPressedCallback
    {
        public CustomBackPressedCallback(bool enabled) : base(enabled) { }

        public override void HandleOnBackPressed()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                bool answer = await Shell.Current.DisplayAlert("Выход", "Закрыть приложение?", "Да", "Нет");
                if (answer)
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            });
        }
    }
}
