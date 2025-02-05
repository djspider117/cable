using Cable.App.ViewModels.Data;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace Cable.App.ViewModels.Windows;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<LibraryItemViewModel> _libraryItems =
    [
        new ("2D Render", "2D"),
        new ("Bounding Box", "2D"),
        new ("Box 2D", "2D"),
        new ("Buffer", "Collection"),
        new ("Change Case", "Collection"),
        new ("Converge", "Collection"),
        new ("Find", "Collection"),
        new ("Join", "Collection"),
        new ("And", "Logic"),
        new ("Or", "Logic"),
        new ("Not", "Logic"),
        new ("Any", "Logic"),
        new ("Bool", "Logic"),
    ];
}
