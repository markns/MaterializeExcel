using System.Reactive.Disposables;
using MahApps.Metro.IconPacks;
using MaterializeExcel.ViewModel.Nodes;
using NLog;
using ReactiveUI;

namespace MaterializeExcel.View.Nodes
{
    public partial class DatabaseNodeView
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