using DynamicData;
using MaterializeExcel.ViewModel.Services;
using MaterializeExcel.ViewModel.Services;

namespace MaterializeExcel.ViewModel.Nodes
{
    public class DatabaseNodeViewModel : NodeViewModel
    {
        public DatabaseNodeViewModel(Node<ICatalogNode,string> node, NodeViewModel parent = null) : base(node, parent)
        {
        }

    }
}