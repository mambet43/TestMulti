using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace TestMulti

{
    public partial class MainPage : ContentPage
    {

        public static MainPage Instance;
        public MainPage()
        {            
            InitializeComponent();
            Instance = this;
            BindingContext = new ViewModels.MainPage(this);    
        }
       
    }

}
