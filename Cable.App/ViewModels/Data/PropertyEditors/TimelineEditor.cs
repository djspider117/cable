using Cable.App.Models.Data;
using Cable.App.Models.Data.Connections;
using System.Windows.Input;

namespace Cable.App.ViewModels.Data.PropertyEditors;

//public partial class TimelineEditor(INodeData parent, string name, Func<CableTimeline> getter, Action<CableTimeline> setter)
//    : PropertyEditor<CableTimeline>(parent, name, getter, setter)
//{
//    public ICommand StartCommand { get; set; } = new RelayCommand(() => getter().Start());
//    public ICommand PauseCommand { get; set; } = new RelayCommand(() => getter().Pause());
//    public ICommand StopCommand { get; set; } = new RelayCommand(() => getter().Stop());

//    public override void PushPropertyChanged()
//    {
        
//    }

//    public override IConnection CreateConnectionAsDestination(INodeData source)
//    {
//        return new TimelineConnection(source, Parent, DisplayName);
//    }

//    public override IConnection CreateConnectionAsSource(INodeData destination)
//    {
//        return new TimelineConnection(Parent, destination, DisplayName);
//    }
//}