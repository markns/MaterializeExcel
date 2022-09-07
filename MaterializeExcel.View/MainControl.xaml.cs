using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using MahApps.Metro.IconPacks;
using MaterializeExcel.ViewModel;
using MaterializeExcel.ViewModel;
using NLog;
using ReactiveUI;

namespace MaterializeExcel.View
{
    public partial class MainControl : ReactiveUserControl<MainControlViewModel>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public MainControl(MainControlViewModel mainControlViewModel)
        {
            InitializeComponent();
            
            // todo: is this the best place?
            // Schedule an no-op action on the main thread, otherwise RxApp.MainThreadScheduler isn't initialized
            // properly when running in Excel add in.
            RxApp.MainThreadScheduler.Schedule(() => { });

            // import pack icon material else dependency is not compiled into assembly
            // https://stackoverflow.com/a/40014591/57215
            {
                var unused = new PackIconMaterial();
            }
            {
                var unused = new PackIconFontAwesome();
            }
            
            this
                .WhenActivated(
                    disposables =>
                    {
                        ViewModel = mainControlViewModel;

                        CatalogTree.ItemsSource = ViewModel?.Catalog.CatalogNodeViewModels;

                        // Disposable
                        //     .Create(
                        //         () =>
                        //         {
                        //             // CommandManager.RemovePreviewCanExecuteHandler(this.tweetTextTextBox, this.OnTweetTextBoxPreviewCanExecute);
                        //             // CommandManager.RemovePreviewExecutedHandler(this.tweetTextTextBox, this.OnTweetTextBoxPreviewExecuted);
                        //         })
                        //     .DisposeWith(disposables);

                        // this
                        //     .ViewModel
                        //     .Errors
                        //     .SelectMany(error => this.ShowMessage("Sorry, something has gone wrong", error))
                        //     .Subscribe()
                        //     .DisposeWith(disposables);

                    });


        }
    }
}