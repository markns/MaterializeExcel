using System.Reactive.Disposables;
using MaterializeExcelViewModel;
using MaterializeExcelViewModel.Nodes;
using NLog;
using ReactiveUI;

namespace MaterializeExcel.View.Nodes
{
    public partial class SchemaNodeView : ReactiveUserControl<SchemaNodeViewModel>
    {
        public SchemaNodeView()
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