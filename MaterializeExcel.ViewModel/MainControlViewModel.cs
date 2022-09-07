using System;
using MaterializeExcel.ViewModel;
using ReactiveUI;

namespace MaterializeExcel.ViewModel
{
    
    public class MainControlViewModel : ReactiveObject
    {
        private readonly IObservable<string> errors;
        
        public MainControlViewModel(CatalogViewModel catalogViewModel)
        {
            Catalog = catalogViewModel;
        }

        public CatalogViewModel Catalog { get; }

        public IObservable<string> Errors => this.errors;
        

    }
}