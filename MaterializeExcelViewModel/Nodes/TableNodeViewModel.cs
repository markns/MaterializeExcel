using DynamicData;
using MaterializeExcelViewModel.Services;

namespace MaterializeExcelViewModel.Nodes
{
    public class TableNodeViewModel : NodeViewModel
    {
        public TableNodeViewModel(Node<ICatalogNode,string> node, NodeViewModel parent = null) : base(node, parent)
        {
        }

    }
}