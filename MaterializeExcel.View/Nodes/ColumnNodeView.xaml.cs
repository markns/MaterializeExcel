using System.Reactive.Disposables;
using MaterializeExcel.ViewModel.Nodes;
using NLog;
using ReactiveUI;

namespace MaterializeExcel.View.Nodes
{
    public partial class ColumnNodeView
    {
        public ColumnNodeView()
        {
            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    this.OneWayBind(ViewModel,
                            x => x.Name,
                            x => x.ObjectName.Content)
                        .DisposeWith(disposables);
                    this.OneWayBind(ViewModel,
                            x => x.Type,
                            x => x.Type.Content)
                        .DisposeWith(disposables);
                });
        }
    }
}