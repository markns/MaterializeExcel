using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using DynamicData;
using MaterializeExcelViewModel.Services;
using NLog;
using NLog.Fluent;
using ReactiveUI;

namespace MaterializeExcelViewModel
{
    public class CatalogViewModel : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        private readonly CatalogService _catalogService;
        private readonly ReadOnlyObservableCollection<CatalogNodeViewModel> _catalogNodeViewModels;
        private readonly IDisposable _cleanUp;

        public CatalogViewModel(CatalogService catalogService, IScheduler rxApp)
        {
            _catalogService = catalogService;

            //transform the data to a full nested tree
            //then transform into a fully recursive view model
            _cleanUp = catalogService.CatalogNodes.Connect()
                .TransformToTree(node => node.OwnerId)
                .Transform(node => new CatalogNodeViewModel(node))
                .Do(x => Logger.Info($"thread {Thread.CurrentThread.ManagedThreadId} before observe on: {x}"))
                .ObserveOn(rxApp)
                .Do(x => Logger.Info($"thread {Thread.CurrentThread.ManagedThreadId} after observe on: {x}"))
                .Bind(out _catalogNodeViewModels)
                .DisposeMany()
                .Subscribe(
                    changeSet => Logger.Info($"next: {changeSet}"),
                    ex => Logger.Info($"error: {ex.Message}")
                    // () => Logger.Info("finished")
                );
        }

        // private string _TextName;
        //
        // public string TextName
        // {
        //     get { return _TextName; }
        //     set { this.RaiseAndSetIfChanged(ref _TextName, value); }
        // }
        //

        //
        // private int _Progress;
        //
        // public int Progress
        // {
        //     get { return _Progress; }
        //     set { this.RaiseAndSetIfChanged(ref _Progress, value); }
        // }
        //
        // public ReactiveCommand<Unit, AsyncVoid> StartAsyncCommand { get; protected set; }
        
        // private void Promote(EmployeeViewModel viewModel)
        // {
        //     if (!viewModel.Parent.HasValue) return;
        //     _employeeService.Promote(viewModel.Dto,viewModel.Parent.Value.BossId);
        // }
        //
        // private void Sack(EmployeeViewModel viewModel)
        // {
        //     _employeeService.Sack(viewModel.Dto);
        // }

        public ReadOnlyObservableCollection<CatalogNodeViewModel> CatalogNodeViewModels => _catalogNodeViewModels;

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}