using System.Reactive.Disposables;
using MahApps.Metro.IconPacks;
using MaterializeExcelViewModel.Nodes;
using NLog;
using ReactiveUI;

namespace MaterializeExcel.View.Nodes
{
    public partial class DatabaseNodeView : ReactiveUserControl<DatabaseNodeViewModel>
    {
        public DatabaseNodeView()
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