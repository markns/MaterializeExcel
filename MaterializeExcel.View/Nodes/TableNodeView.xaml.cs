using System.Reactive.Disposables;
using MaterializeExcel.ViewModel;
using MaterializeExcel.ViewModel.Nodes;
using NLog;
using ReactiveUI;

namespace MaterializeExcel.View.Nodes
{
    public partial class ObjectNodeView : ReactiveUserControl<TableNodeViewModel>
    {
        public ObjectNodeView()
        {
            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    this.OneWayBind(ViewModel,
                            x => x.Name,
                            x => x.ObjectName.Content)
                        .DisposeWith(disposables);

                    this
                        .BindCommand(this.ViewModel,
                            x => x.AddToSheetCommand,
                            x => x.AddToSheetButton)
                        .DisposeWith(disposables);
                });
        }
    }
}