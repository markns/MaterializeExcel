using DynamicData;
using MaterializeExcel.ViewModel.Services;
using MaterializeExcel.ViewModel.Services;
using ReactiveUI;

namespace MaterializeExcel.ViewModel.Nodes
{
    public class SourceNodeViewModel : NodeViewModel
    {
        public SourceNodeViewModel(Node<ICatalogNode, string> node, IMessageBus messageBus, NodeViewModel parent = null)
            : base(node, messageBus, parent)
        {
        }
    }
}