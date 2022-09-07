using System.Reactive.Disposables;
using ReactiveUI;

namespace MaterializeExcel.View.Nodes
{
    public partial class SchemaNodeView
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