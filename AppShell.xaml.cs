using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using TestMulti.Constants;
using TestMulti.Models;
using TestMulti.Services;
using TestMulti.ViewModels;
using TestMulti.Views;
using static SkiaSharp.HarfBuzz.SKShaper;


namespace TestMulti
{
    public partial class AppShell : Shell
    {
        public static AppShell Instance;

        public AppShell()
        {
            InitializeComponent();
            Instance = this;
            var menuItemVaworites = new MenuItem
            {
                Text = "Избранные вопросы",
                IconImageSource = ImageSource.FromResource("TestMulti.Resources.Images.star.png"),
                Command = new Command<string>(param => MenuItemCommand(param)),
                CommandParameter = "vaworites"
            };
            Items.Add(menuItemVaworites);
            // Preferences.Clear();
            if (Preferences.Get("ThemesDict", null) == null)
            {
                try
                {
                    // Получаем путь к папке AppData
                    string appDataPath = FileSystem.Current.AppDataDirectory;
                    var dict = new Dictionary<string, string>(AppConstants.ThemesDict);

                    foreach (var kvp in dict)
                    {
                        string destinationPath = Path.Combine(appDataPath, kvp.Key);
                        // Проверяем, существует ли файл в ресурсах
                        var assembly = Assembly.GetExecutingAssembly();
                        var resourceName = $"TestMulti.Resources.Raw.{kvp.Key}";

                        using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
                        {
                            if (resourceStream != null)
                            {
                                using (FileStream fileStream = File.Create(destinationPath))
                                {
                                    resourceStream.CopyToAsync(fileStream);
                                    Task.Delay(100).Wait(); // Ждем завершения копирования
                                    string title = kvp.Value;
                                    AppConstants.ThemesDict.Remove(kvp.Key);
                                    AppConstants.ThemesDict.Add(Path.Combine(FileSystem.Current.AppDataDirectory, kvp.Key), title);
                                }
                            }                            
                        }
                    }
                    Preferences.Set("ThemesDict", JsonSerializer.Serialize(AppConstants.ThemesDict));

                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Ошибка при копировании файла: {ex.Message}");
                }
            }    
              


            AppConstants.ThemesDict = JsonSerializer.Deserialize<Dictionary<string, string>>(Preferences.Get("ThemesDict", null));
            foreach (KeyValuePair<string, string> kvp in AppConstants.ThemesDict)
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
            Routing.RegisterRoute("QuestTheme", typeof(QuestTheme));
        }


        public async void MenuItemCommand(string file)
        {
            string encodedFile = Uri.EscapeDataString(file);

            await Shell.Current.GoToAsync("QuestTheme?file="+ encodedFile, false);
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
                    Theme.IsChange = false;
                }     
            }            
        }


    }
}
