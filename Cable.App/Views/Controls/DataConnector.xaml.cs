using System;
using System.Collections.Generic;
using System.Linq;
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

public partial class DataConnector : UserControl
{
    public DataConnector()
    {
        InitializeComponent();
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        e.Handled = true;
        base.OnMouseEnter(e);
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        e.Handled = true;
        base.OnMouseDown(e);
    }
}
