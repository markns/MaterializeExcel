using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Forms;
using DynamicData;
using DynamicData.Binding;
using DynamicData.Kernel;
using MaterializeExcel.Events;
using MaterializeExcelViewModel.Services;
using NLog;
using ReactiveUI;

namespace MaterializeExcelViewModel
{
    public class CatalogNodeViewModel : ReactiveObject, IDisposable, IEquatable<CatalogNodeViewModel>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        private readonly IDisposable _cleanUp;
        private bool _isExpanded;
        private bool _isSelected;
        
        private ReadOnlyObservableCollection<CatalogNodeViewModel> _children;
        
        public CatalogNodeViewModel(Node<ICatalogNode,string> node, CatalogNodeViewModel parent = null)
        {
            Logger.Info($"Creating view model for {node.Item} {node.Item.Name}");
            
            Id = node.Key;
            Name = node.Item.Name;
            Depth = node.Depth;
            Parent = parent;
            OwnerId = node.Item.OwnerId;
            
            //Wrap loader for the nested view model inside a lazy so we can control when it is invoked
            var childrenLoader = new Lazy<IDisposable>(() => node.Children.Connect()
                .Transform(e => new CatalogNodeViewModel(e, this))
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

            // todo: what does the WhenAny do here? 
            AddToSheetCommand = ReactiveCommand.Create(
                () => this.WhenAny(x => x.Name, 
                    x => !string.IsNullOrEmpty(x.Value)));
            AddToSheetCommand.Subscribe(_ =>
            {
                Logger.Info($"You clicked on AddToSheetCommand: Name is {Name}");
                MessageBus.Current.SendMessage(new AddToSheetEvent(Name));
            });
            
            _cleanUp = Disposable.Create(() =>
            {
                AddToSheetCommand.Dispose();
                expander.Dispose();
                if (childrenLoader.IsValueCreated)
                    childrenLoader.Value.Dispose();
            });
            
        }
        
        public string Id { get; }

        public string Name { get; }

        public int Depth { get; }

        public string OwnerId { get; }

        public Optional<CatalogNodeViewModel> Parent { get; }
        
        public ReadOnlyObservableCollection<CatalogNodeViewModel> Children => _children;
        
        
        public ReactiveCommand<Unit, IObservable<bool>> AddToSheetCommand { get; protected set; }
        
        // todo: use fody weavers
        public bool IsExpanded
        {
            get => _isExpanded;
            set => this.RaiseAndSetIfChanged(ref _isExpanded, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => this.RaiseAndSetIfChanged(ref _isSelected, value);
        }
        
        #region Equality Members
        
        public bool Equals(CatalogNodeViewModel other)
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
            return Equals((CatalogNodeViewModel)obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }

        public static bool operator ==(CatalogNodeViewModel left, CatalogNodeViewModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CatalogNodeViewModel left, CatalogNodeViewModel right)
        {
            return !Equals(left, right);
        }
        #endregion

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}