using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMulti.Models
{
    public class Answer : INotifyPropertyChanged
    {
        public string title { get; set; }
        public bool correct { get; set; }



        private string _backgroundColorHex;
        public string BackgroundColorHex
        {
            get => _backgroundColorHex;
            set
            {
                if (_backgroundColorHex != value)
                {
                    _backgroundColorHex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundColorHex)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundColor)));
                }
            }
        }

        // Вспомогательное свойство для привязки в XAML
        public Color BackgroundColor => Color.FromArgb(BackgroundColorHex);

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
