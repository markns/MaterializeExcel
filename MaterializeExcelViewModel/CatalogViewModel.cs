﻿using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using MaterializeExcelViewModel.Nodes;
using MaterializeExcelViewModel.Services;
using NLog;
using NLog.Fluent;
using NLog.Targets;
using ReactiveUI;

namespace MaterializeExcelViewModel
{
    public class CatalogViewModel : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        private readonly CatalogService _catalogService;
        private readonly ReadOnlyObservableCollection<NodeViewModel> _catalogNodeViewModels;
        private readonly IDisposable _cleanUp;

        public CatalogViewModel(CatalogService catalogService)
        {
            _catalogService = catalogService;

            
            //transform the data to a full nested tree
            //then transform into a fully recursive view model
            _cleanUp = catalogService.CatalogNodes.Connect()
                .TransformToTree(node => node.OwnerId)
                .Transform(n => NodeViewModel.GetViewModelForNode(n))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _catalogNodeViewModels)
                .DisposeMany()
                .Subscribe(
                    changeSet => Logger.Info($"next: {changeSet}"),
                    ex => Logger.Info($"error: {ex.Message}"),
                    () => Logger.Info("finished")
                );


        }
        
        public ReadOnlyObservableCollection<NodeViewModel> CatalogNodeViewModels => _catalogNodeViewModels;

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}