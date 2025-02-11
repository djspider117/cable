using Cable.App.ViewModels.Data;

namespace Cable.App.Views.Controls;

public interface INodeViewResolver
{
    NodeView? GetViewFromViewModel(NodeViewModel? vm);
}
