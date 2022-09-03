using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using MaterializeExcelViewModel;
using NLog;


namespace MaterializeExcelView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly MainControlViewModel _viewModel = new MainControlViewModel();


        public MainWindow()
        {
            Logger.Info($"-------------------- Starting main view");
            
            InitializeComponent();
            DataContext = _viewModel;
        }
        
    }
}