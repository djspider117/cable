using Cable.App.Views.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cable.App.Models.Messages;

public record StartNodeConnectionMessage(NodeView SourceView);
