using System;
using System.Diagnostics;
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
            var objectNode = node.Item as ObjectNode;

            // todo: better way to handle this?
            Debug.Assert(objectNode != null, nameof(objectNode) + " != null");

            AddToSheetCommand = ReactiveCommand.Create(
                () => this.WhenAny(x => x.Name,
                    x => !string.IsNullOrEmpty(x.Value)));
            AddToSheetCommand.Subscribe(_ =>
            {
                Logger.Debug($"Executing AddToSheetCommand: {objectNode.Database}.{objectNode.Schema}.{Name}");
                messageBus.SendMessage(new AddToSheetRequest(objectNode.Database, objectNode.Schema, Name));
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