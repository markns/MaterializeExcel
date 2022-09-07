using MaterializeExcel.ViewModel;
using MaterializeExcel.ViewModel;
using ReactiveUI;

namespace MaterializeExcel.TestApp
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainControlViewModel MainControlViewModel { get; }
        // public RoutingState Router { get; private set; }
        
        public MainWindowViewModel(MainControlViewModel mainControlViewModel)
        {
            // Router = new RoutingState();
            MainControlViewModel = mainControlViewModel;

            // var canGoBack = this.WhenAnyValue(vm => vm.Router.NavigationStack.Count)
            //     .Select(count => count > 0);
            //
            // NavigateToACommand = ReactiveCommand.Create();
            // NavigateToBCommand = ReactiveCommand.Create();
            // BackCommand = ReactiveCommand.Create(canGoBack);
            //
            // NavigateToACommand.Subscribe(NavigateToA);
            // NavigateToBCommand.Subscribe(NavigateToB);
            // BackCommand.Subscribe(NavigateBack);
        }
    }
}