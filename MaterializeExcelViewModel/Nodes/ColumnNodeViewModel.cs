using System.Diagnostics;
using DynamicData;
using MaterializeExcelViewModel.Services;

namespace MaterializeExcelViewModel.Nodes
{
    public class ColumnNodeViewModel : NodeViewModel
    {
        public string Type { get; }
        
        public ColumnNodeViewModel(Node<ICatalogNode,string> node, NodeViewModel parent = null) : base(node, parent)
        {
            var columnNode = node.Item as ColumnNode;

            // todo: better way to handle this?
            Debug.Assert(columnNode != null, nameof(columnNode) + " != null");
            Type = $"({columnNode.Type.ToUpper()}{(columnNode.Nullable ? " NULL" : "")})";
        }

    }
}