using CommunityToolkit.Mvvm.ComponentModel;

namespace TestMulti.Models
{
    public partial class Answer : ObservableObject
    {
        public string title { get; set; }
        public bool correct { get; set; }

        [ObservableProperty]
        private string backgroundColorHex = "#00FFFFFF";

        // Вспомогательное свойство для привязки в XAML
        public Color BackgroundColor => Color.FromArgb(BackgroundColorHex);

        partial void OnBackgroundColorHexChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(nameof(BackgroundColor));
        }
    }
}