using DynamicData;
using MaterializeExcelViewModel.Services;

namespace MaterializeExcelViewModel.Nodes
{
    public class SchemaNodeViewModel : NodeViewModel
    {
        public SchemaNodeViewModel(Node<ICatalogNode,string> node, NodeViewModel parent = null) : base(node, parent)
        {
        }

    }
}