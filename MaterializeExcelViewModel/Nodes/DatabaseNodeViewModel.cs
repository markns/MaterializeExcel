using DynamicData;
using MaterializeExcelViewModel.Services;

namespace MaterializeExcelViewModel.Nodes
{
    public class DatabaseNodeViewModel : NodeViewModel
    {
        public DatabaseNodeViewModel(Node<ICatalogNode,string> node, NodeViewModel parent = null) : base(node, parent)
        {
        }

    }
}