using System.Reactive.Concurrency;
using MaterializeExcelViewModel;
using NLog;

namespace MaterializeExcel.View
{
    public partial class MainControl 
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        // public MainControl()
        // {
        //     Logger.Info($"-------------------- Starting main control");
        //     
        //     InitializeComponent();
        //     DataContext = _viewModel;        }

        public MainControl(IScheduler mainThreadScheduler)
        {
            DataContext = new MainControlViewModel(mainThreadScheduler);
            // throw new System.NotImplementedException();
        }
    }
}