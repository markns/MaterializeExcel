using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using DynamicData.Kernel;
using MaterializeExcel.Events;
using MaterializeExcel.ViewModel.Services;
using MaterializeExcel.ViewModel.Services;
using NLog;
using ReactiveUI;

namespace MaterializeExcel.ViewModel.Nodes
{
    public abstract class NodeViewModel : ReactiveObject, IEquatable<NodeViewModel>, IDisposable
    {
        private IDisposable _cleanUp;
        private bool _isExpanded;
        private bool _isSelected;
        private ReadOnlyObservableCollection<NodeViewModel> _children;

        protected NodeViewModel(Node<ICatalogNode, string> node, IMessageBus messageBus, 
            NodeViewModel parent = null)
        {
            Id = node.Key;
            Name = node.Item.Name;
            Depth = node.Depth;
            Parent = parent;
            OwnerId = node.Item.OwnerId;
            
            //Wrap loader for the nested view model inside a lazy so we can control when it is invoked
            var childrenLoader = new Lazy<IDisposable>(() => node.Children.Connect()
                .Transform(n => GetViewModelForNode(n, messageBus, this))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _children)
                .DisposeMany()
                .Subscribe());
            
            //return true when the children should be loaded 
            //(i.e. if current node is a root, otherwise when the parent expands)
            var shouldExpand = node.IsRoot
                ? Observable.Return(true)
                : Parent.Value.WhenValueChanged(thisOne => thisOne.IsExpanded);
            
            // //wire the observable
            var expander = shouldExpand
                .Where(isExpanded => isExpanded)
                .Take(1)
                .Subscribe(_ =>
                {
                    //force lazy loading
                    var x = childrenLoader.Value;
                });

            _cleanUp = Disposable.Create(() =>
            {

                expander.Dispose();
                if (childrenLoader.IsValueCreated)
                    childrenLoader.Value.Dispose();
            });

            
        }

        public object ViewModel => this;
        public string Id { get; }
        public string Name { get; }
        public int Depth { get; }
        public string OwnerId { get; }
        public Optional<NodeViewModel> Parent { get; }
        public ReadOnlyObservableCollection<NodeViewModel> Children => _children;


        public bool IsExpanded
        {
            get => _isExpanded;
            set => this.RaiseAndSetIfChanged<NodeViewModel, bool>(ref _isExpanded, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => this.RaiseAndSetIfChanged<NodeViewModel, bool>(ref _isSelected, value);
        }

        public bool Equals(NodeViewModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NodeViewModel)(object)(NodeViewModel)obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }

        public static bool operator ==(NodeViewModel left, NodeViewModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NodeViewModel left, NodeViewModel right)
        {
            return !Equals(left, right);
        }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }

        public static NodeViewModel GetViewModelForNode(Node<ICatalogNode, string> node,
            IMessageBus messageBus,
            NodeViewModel parent = null)
        {
            switch (node.Item)
            {
                case DatabaseNode _:
                    return new DatabaseNodeViewModel(node, messageBus, parent);
                case SchemaNode _:
                    return new SchemaNodeViewModel(node, messageBus, parent);
                case ObjectNode _:
                    return new TableNodeViewModel(node, messageBus, parent);
                case ColumnNode _:
                    return new ColumnNodeViewModel(node, messageBus, parent);
                default:
                    throw new ApplicationException("Unknown catalog type");
            }
        }
    }
}