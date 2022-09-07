using System;
using System.Reactive;
using System.Reactive.Disposables;
using DynamicData;
using MaterializeExcel.Events;
using MaterializeExcel.ViewModel.Services;
using NLog;
using ReactiveUI;

namespace MaterializeExcel.ViewModel.Nodes
{
    public class TableNodeViewModel : NodeViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private IDisposable _cleanUp;

        public TableNodeViewModel(Node<ICatalogNode, string> node, IMessageBus messageBus, NodeViewModel parent = null
        ) : base(node, messageBus, parent)
        {
            AddToSheetCommand = ReactiveCommand.Create(
                () => this.WhenAny(x => x.Name,
                    x => !string.IsNullOrEmpty(x.Value)));
            AddToSheetCommand.Subscribe(_ =>
            {
                Logger.Info($"You clicked on AddToSheetCommand: Name is {Name}");
                messageBus.SendMessage(new AddToSheetRequest("parent.Parent.Name",
                    parent?.Name, Name));
            });

            _cleanUp = Disposable.Create(() => { AddToSheetCommand.Dispose(); });
        }

        public ReactiveCommand<Unit, IObservable<bool>> AddToSheetCommand { get; protected set; }

        // todo: how to dispose and make sure parent is also disposed?
        // public new void Dispose()
        // {
        //     base.Dispose();
        //     _cleanUp.Dispose();
        // }
    }
}