using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using TestMulti.Constants;
using TestMulti.Controls;
using TestMulti.Models;
using TestMulti.Views;

namespace TestMulti.ViewModels;

public partial class MainPage : ObservableObject, INotifyPropertyChanged
{

    [ObservableProperty]
    private ObservableCollection<Theme> themes = new ObservableCollection<Theme>();


    [ObservableProperty]
    private int currentPosition;
    public static MainPage Instance { get; private set; }

    private readonly ContentPage mainPage;
    public MainPage(ContentPage page)
    {                    
        Instance = this;
        mainPage = page;        
        Routing.RegisterRoute("QuestionPage", typeof(QuestionPage));
        LoadQuest();
    }

    [ObservableProperty]
    private bool isRefreshing;


    [ObservableProperty]
    private string textEmptyTheme;

    [RelayCommand]
    public async Task RefreshAsync()
    {
        int position = CurrentPosition;
        IsRefreshing = true;        
        await LoadQuest();
        await Task.Delay(500);
        CurrentPosition = position;       
        IsRefreshing = false;
    }


    [RelayCommand]
    private async Task AddTheme()
    {
        try
        {
            var options = new PickOptions
            {
                PickerTitle = "Выберите JSON-файл",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { ".json" } },
                { DevicePlatform.Android, new[] { "application/json" } }
            })
            };

            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                string destPath = Path.Combine(FileSystem.Current.AppDataDirectory, result.FileName);
                ObservableCollection<Quest> quests;
                using (var sourceStream = await result.OpenReadAsync())
                {
                    try
                    {
                        quests = JsonSerializer.Deserialize<ObservableCollection<Quest>>(sourceStream) ?? new ObservableCollection<Quest>();
                        if (quests.Count > 0)
                        {
                            var popup = new AddThemePopup("Введите название темы");
                            var ThemeName = await Shell.Current.CurrentPage.ShowPopupAsync(popup);

                            if (ThemeName is string text && !string.IsNullOrWhiteSpace(text))
                            {
                                // Возвращаем позицию потока в начало перед копированием
                                sourceStream.Seek(0, SeekOrigin.Begin);
                                using (var destStream = File.Create(destPath))
                                {
                                    await sourceStream.CopyToAsync(destStream);
                                }

                                AppConstants.ThemesDict.Add(destPath, text);
                                Preferences.Set(destPath, JsonSerializer.Serialize(quests));
                                Preferences.Set("ThemesDict", JsonSerializer.Serialize(AppConstants.ThemesDict));                               
                                var menuItem = new MenuItem
                                {
                                    Text = text,
                                    Command = new Command<string>(param => AppShell.Instance.MenuItemCommand(param)),
                                    CommandParameter = destPath,
                                    IconImageSource = ImageSource.FromResource("TestMulti.Resources.Images.lib.png")
                                };
                                AppShell.Instance.Items.Add(menuItem);
                                await Shell.Current.DisplayAlert("Успех", $"Файл {result.FileName} успешно добавлен.", "OK");
                            }
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Ошибка", "Файл не содержит вопросов.", "OK");
                        }
                    }
                    catch (JsonException jsonEx)
                    {
                        await Shell.Current.DisplayAlert("Ошибка JSON", $"Неверный формат файла: {jsonEx.Message}", "OK");
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Ошибка", $"Ошибка при обработке файла: {ex.Message}", "OK");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", $"Общая ошибка: {ex.Message}", "OK");
        }
        await LoadQuest();
    }

    [RelayCommand]
    private async void DelTheme()
    {
        if (themes.Count == 0 || CurrentPosition < 0 || CurrentPosition >= themes.Count)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Нет доступных тем для удаления.", "ОК");
            return;
        }
        bool result = await Shell.Current.DisplayAlert(
                            "Подтверждение",
                            "Вы точно хотите удалить тему, отменить это действие нельзя?",
                            "Да",
                            "Отмена");
        if (result )
        {
            string filePath = themes[CurrentPosition].FileName;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                AppConstants.ThemesDict.Remove(filePath);
            }
            Preferences.Remove(filePath);
            AppConstants.ThemesDict.Remove(filePath);
            Preferences.Set("ThemesDict", JsonSerializer.Serialize(AppConstants.ThemesDict));
            foreach (var item in AppShell.Instance.Items)
            {
                if (item.Title == themes[CurrentPosition].Title)
                {
                    AppShell.Instance.Items.Remove(item);
                    break;
                }
            }
            await LoadQuest();
        }    
           
    }

    [RelayCommand]
    private async void ShareTheme()
    {
        if (themes.Count == 0 || CurrentPosition < 0 || CurrentPosition >= themes.Count)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Нет доступных тем для отправки", "ОК");
            return;
        }
        string filePath = themes[CurrentPosition].FileName;
        if (File.Exists(filePath))
        {
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Поделиться файлом темы",
                File = new ShareFile(filePath)
            });
        }
        else
        {
            await Shell.Current.DisplayAlert("Ошибка", "Файл не найден", "ОК");
        }
    }

    


    public async Task LoadQuest()
    {       
        if (AppConstants.ThemesDict.Count > 0)
        {
            TextEmptyTheme = string.Empty;
            var newThemes = new List<Theme>();
            foreach (var kvp in AppConstants.ThemesDict)
            {
                newThemes.Add(await Theme.CreateAsync(kvp.Key, kvp.Value));
            }
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Themes = new ObservableCollection<Theme>(newThemes);
            });
        }
        else
        {
            TextEmptyTheme = "Нет тем для тестирования. Добавьте тему для тестов!";
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Themes = new ObservableCollection<Theme>();
            });
        }
    }






}