using System.Reactive.Disposables;
using NLog;
using ReactiveUI;


namespace MaterializeExcel.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            ViewModel = mainWindowViewModel;
            
            Logger.Info($"-------------------- Starting main view {ViewModel}");

            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .OneWayBind(this.ViewModel,
                                x => x.MainControlViewModel, 
                                x => x.Host.ViewModel)
                            .DisposeWith(disposables);
                    });
            
        }
    }
}