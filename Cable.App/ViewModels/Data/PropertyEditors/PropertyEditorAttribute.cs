namespace Cable.App.ViewModels.Data.PropertyEditors;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public sealed class PropertyEditorAttribute<T> : Attribute where T : IPropertyEditor
{

}
