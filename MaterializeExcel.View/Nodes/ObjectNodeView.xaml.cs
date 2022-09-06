using System.Reactive.Disposables;
using MaterializeExcelViewModel;
using MaterializeExcelViewModel.Nodes;
using NLog;
using ReactiveUI;

namespace MaterializeExcel.View.Nodes
{
    public partial class ObjectNodeView : ReactiveUserControl<ObjectNodeViewModel>
    {
        public ObjectNodeView()
        {
            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    this.OneWayBind(ViewModel,
                            x => x.Name,
                            x => x.ObjectName.Text)
                        .DisposeWith(disposables);
                });
        }
    }
}