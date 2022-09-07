using DynamicData;
using MaterializeExcelViewModel.Services;

namespace MaterializeExcelViewModel.Nodes
{
    public class SourceNodeViewModel : NodeViewModel
    {
        public SourceNodeViewModel(Node<ICatalogNode,string> node, NodeViewModel parent = null) : base(node, parent)
        {
        }

    }
}