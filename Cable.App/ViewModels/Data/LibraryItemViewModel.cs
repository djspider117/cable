namespace Cable.App.ViewModels.Data;

public partial class LibraryItemViewModel : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private bool _isFavorite;

    [ObservableProperty]
    private string _category = string.Empty;

    public LibraryItemViewModel(string name, string categ)
    {
        _name = name;
        _category = categ;
    }

    public override string ToString() => Name;
}
