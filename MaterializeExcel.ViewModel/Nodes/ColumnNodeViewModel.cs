using System.Diagnostics;
using DynamicData;
using MaterializeExcel.ViewModel.Services;
using MaterializeExcel.ViewModel.Services;
using ReactiveUI;

namespace MaterializeExcel.ViewModel.Nodes
{
    public class ColumnNodeViewModel : NodeViewModel
    {
        public string Type { get; }
        
        public ColumnNodeViewModel(Node<ICatalogNode,string> node, IMessageBus messageBus, NodeViewModel parent = null) : base(node, messageBus, parent)
        {
            var columnNode = node.Item as ColumnNode;

            // todo: better way to handle this?
            Debug.Assert(columnNode != null, nameof(columnNode) + " != null");
            Type = $"({columnNode.Type.ToUpper()}{(columnNode.Nullable ? " NULL" : "")})";
        }

    }
}