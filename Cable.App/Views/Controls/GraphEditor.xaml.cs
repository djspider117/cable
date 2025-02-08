using Cable.App.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cable.App.Views.Controls;
/// <summary>
/// Interaction logic for GraphEditor.xaml
/// </summary>
public partial class GraphEditor : UserControl
{
    public GraphEditor()
    {
        InitializeComponent();

        var numberNode = new FloatNode(10.2f);
        var vectorNode = new Float2Node(new Vector2(5,12));
        var transformNode = new Transform2DNode();

        var c1 = new FloatConnection(numberNode, transformNode);
        var c2 = new Float2Connection(vectorNode, transformNode);

        transformNode.RotationConnection = c1;
        transformNode.TranslationConnection = c2;

        pnlNodeContainer.Children.Add(new NodeView() { ViewModel = new NodeViewModel(numberNode) });
        pnlNodeContainer.Children.Add(new NodeView() { ViewModel = new NodeViewModel(vectorNode) });
        pnlNodeContainer.Children.Add(new NodeView() { ViewModel = new NodeViewModel(transformNode) });
    }
}
