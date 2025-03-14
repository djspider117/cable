using Cable.App.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cable.App.ViewModels.Data;
public partial class CableSceneViewModel : ObservableObject
{
    [ObservableProperty]
    private CableTimeline _timeline;

    public CableSceneViewModel()
    {
        _timeline = new CableTimeline();
    }
}
