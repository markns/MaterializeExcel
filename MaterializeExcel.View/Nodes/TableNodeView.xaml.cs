using System.Reactive.Disposables;
using MaterializeExcelViewModel;
using MaterializeExcelViewModel.Nodes;
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
                });
        }
    }
}