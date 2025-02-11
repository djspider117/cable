using Cable.App.ViewModels.Data.PropertyEditors;
using System.Windows.Controls;

namespace Cable.App.Views.TemplateSelectors;

public class PropertyEditorTemplateSelector : DataTemplateSelector
{
    public DataTemplate? InputOnlyEditorTemplate { get; set; }

    public DataTemplate? FloatEditorTemplate { get; set; }
    public DataTemplate? Float2EditorTemplate { get; set; }
    public DataTemplate? ColorEditorTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is FloatEditor)
            return FloatEditorTemplate;

        if (item is Float2Editor)
            return Float2EditorTemplate;

        if (item is ColorEditor)
            return ColorEditorTemplate;

        return InputOnlyEditorTemplate;
    }
}
