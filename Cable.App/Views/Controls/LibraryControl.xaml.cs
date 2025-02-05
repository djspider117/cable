using Cable.App.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

public partial class LibraryControl : UserControl
{
    public static readonly DependencyProperty LibraryItemsProperty =
        DependencyProperty.Register(nameof(LibraryItems), typeof(ObservableCollection<LibraryItemViewModel>), typeof(LibraryControl), new PropertyMetadata(null, (d, e) => ((LibraryControl)d).OnLibraryItemsChanged(e)));

    public ObservableCollection<LibraryItemViewModel> LibraryItems
    {
        get => (ObservableCollection<LibraryItemViewModel>)GetValue(LibraryItemsProperty);
        set => SetValue(LibraryItemsProperty, value);
    }

    public LibraryControl()
    {
        InitializeComponent();
    }

    private void OnLibraryItemsChanged(DependencyPropertyChangedEventArgs e)
    {
        var view = (CollectionView)CollectionViewSource.GetDefaultView(LibraryItems);
        var groupDescription = new PropertyGroupDescription(nameof(LibraryItemViewModel.Category));
        view.GroupDescriptions.Add(groupDescription);
        lvLibItems.ItemsSource = view;
    }
}
