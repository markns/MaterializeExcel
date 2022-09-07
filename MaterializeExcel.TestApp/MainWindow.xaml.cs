using System.Reactive.Disposables;
using ReactiveUI;


namespace MaterializeExcel.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            ViewModel = mainWindowViewModel;
            
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