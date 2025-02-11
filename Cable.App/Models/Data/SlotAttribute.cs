using Cable.App.ViewModels.Data.PropertyEditors;

namespace Cable.App.Models.Data;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
sealed class SlotAttribute<TFieldType, TEditorType> : Attribute where TEditorType : IPropertyEditor
{
	public SlotAttribute(string propName)
	{

	}
}

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
sealed class SlotAttribute<TFieldType> : Attribute
{
    public SlotAttribute(string propName)
    {

    }
}