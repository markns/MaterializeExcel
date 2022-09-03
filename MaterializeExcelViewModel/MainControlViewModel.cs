using System.Reactive.Concurrency;
using MaterializeClient;
using MaterializeExcelViewModel.Services;
using ReactiveUI;

namespace MaterializeExcelViewModel
{
    public class MainControlViewModel : ReactiveObject
    {
        public MainControlViewModel(IScheduler rxApp)
        {
            var mzClient = new MzClient("Marks-MacBook-Pro.local", 6875,
                "materialize", "materialize");

            Catalog = new CatalogViewModel(new CatalogService(mzClient), rxApp);
        }

        public CatalogViewModel Catalog { get; }


    }
}