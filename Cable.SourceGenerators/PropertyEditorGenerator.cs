using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

namespace Cable.SourceGenerators;

[Generator]
public class PropertyEditorGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(FilterSymbols,
                transform: static (context, _) =>
                {
                    var classSyntax = (ClassDeclarationSyntax)context.Node;
                    var model = context.SemanticModel;
                    var classSymbol = model.GetDeclaredSymbol(classSyntax) as INamedTypeSymbol;
                    return classSymbol;
                })
            .Where(static info => info is not null)
            .Collect();

        context.RegisterSourceOutput(classDeclarations, CreateSources);
    }

    private bool FilterSymbols(SyntaxNode node, CancellationToken ctok)
    {
        if (node is not ClassDeclarationSyntax cdecl)
            return false;

        var isNodeData = cdecl?.AttributeLists.SelectMany(x => x.Attributes)?.Any(x => x.Name?.ToString()?.Contains("NodeData") ?? false) ?? false;
        var shouldExclude = cdecl?.AttributeLists.SelectMany(x => x.Attributes)?.Any(x => x.Name?.ToString()?.Contains("ExcludeFromSourceGeneration") ?? false) ?? false;

        return !shouldExclude && isNodeData;
    }
    private void CreateSources(SourceProductionContext ctx, ImmutableArray<INamedTypeSymbol?> classes)
    {
        foreach (var classSymbol in classes)
        {
            if (classSymbol is null)
                continue;

            var generatedCode = GeneratePropertyEditorMethod(classSymbol);
            if (generatedCode == null)
                continue;

            ctx.AddSource($"{classSymbol.Name}.g.cs", SourceText.From(generatedCode, Encoding.UTF8));
        }
    }

    private static string? GeneratePropertyEditorMethod(INamedTypeSymbol classSymbol)
    {
        if (classSymbol?.BaseType is null)
            return null;

        var className = classSymbol.Name;
        var baseType = classSymbol.BaseType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
        var fields = classSymbol
            .GetMembers()
            .OfType<IFieldSymbol>()
            .Where(field => field.GetAttributes().Any(a => a.AttributeClass?.Name == "PropertyEditorAttribute"))
            .ToList();

        var classAttributes = classSymbol.GetAttributes();
        var nodeDataAttribute = classAttributes.FirstOrDefault(x => x.AttributeClass?.Name.Contains("NodeData") ?? false)!;

        if (classSymbol.IsGenericType)
        {
            var typeArguments = classSymbol.TypeParameters
                .Select(tp => tp.Name)
                .ToArray();
            className = $"{classSymbol.Name}<{string.Join(", ", typeArguments)}>";
        }

        var sb = new StringBuilder();
        sb.AppendLine("#pragma warning disable");
        sb.AppendLine("using Cable.App.Extensions;");
        sb.AppendLine("using Cable.App.Models.Data;");
        sb.AppendLine("using Cable.App.Models.Data.Connections;");
        sb.AppendLine("using Cable.App.ViewModels.Data.PropertyEditors;");
        sb.AppendLine();
        sb.AppendLine($"namespace {namespaceName}");
        sb.AppendLine("{");
        sb.AppendLine($"    public partial class {className} : {baseType}");
        sb.AppendLine($"    {{");
        sb.AppendLine();

        if (nodeDataAttribute.ConstructorArguments.Length > 0)
        {
            var nameConstant = nodeDataAttribute.ConstructorArguments[0]!;
            var inConstant = nodeDataAttribute.ConstructorArguments[1]!;
            var outConstant = nodeDataAttribute.ConstructorArguments[2];

            sb.AppendLine($"        public {className}() : base(\"{nameConstant.Value}\", ({inConstant.Type?.ToDisplayString()}){inConstant.Value}, ({outConstant.Type?.ToDisplayString()}){outConstant.Value}) {{}}");
        }
        sb.AppendLine();

        List<string> editors = [];

        foreach (var classAttrib in classSymbol.GetAttributes())
        {
            if (!(classAttrib.AttributeClass?.Name == "SlotAttribute" && classAttrib.AttributeClass?.IsGenericType is true))
                continue;

            var ta = classAttrib.AttributeClass.TypeArguments;
            var slotDataType = ta[0];
            var slotEditorType = ta.Length > 1 ? ta[1] : null;
            var slotName = classAttrib.ConstructorArguments.FirstOrDefault().Value?.ToString() ?? string.Empty;
            var fieldName = slotName?.Fieldify() ?? string.Empty;

            sb.AppendLine($"        private {slotDataType}  {fieldName};");
            AddAutoGenStuff(sb, slotName!, fieldName, slotDataType, slotEditorType?.ToDisplayString());

            editors.Add($"{slotName}Editor");
        }

        foreach (var field in fields ?? [])
        {
            var fieldName = field.Name;
            var propName = field.Name.Capitalize();
            var editorType = field.GetAttributes()
                .FirstOrDefault(a => a.AttributeClass?.Name == "PropertyEditorAttribute")
                ?.AttributeClass?.TypeArguments.FirstOrDefault()?.ToDisplayString() ?? "UnknownEditor";

            editors.Add($"{propName}Editor");
            AddAutoGenStuff(sb, propName, fieldName, field.Type, editorType);
        }

        sb.AppendLine();
        sb.AppendLine("        public override IEnumerable<IPropertyEditor> GetPropertyEditors()");
        sb.AppendLine("        {");
        if (editors.Count == 0)
        {
            sb.AppendLine($"            return new List<IPropertyEditor>();");
        }
        else
        {
            foreach (var editor in editors)
            {
                sb.AppendLine($"            yield return {editor};");
            }
        }

        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        return sb.ToString();
    }

    private static void AddAutoGenStuff(StringBuilder sb, string propName, string fieldName, ITypeSymbol fieldType, string? editorType)
    {
        editorType ??= "InputOnlyEditor";

        var connectionName = $"{propName}Connection";
        sb.AppendLine($"        private IConnection<{fieldType.ToDisplayString()}>? _{connectionName};");
        sb.AppendLine($"        private {editorType}? _{propName}Editor;");
        sb.AppendLine($"        public {fieldType.ToDisplayString()} {propName} => {connectionName} == null ? {fieldName} : {connectionName}.GetValue();");
        sb.AppendLine($"        public IConnection<{fieldType.ToDisplayString()}>? {connectionName}");
        sb.AppendLine($"        {{");
        sb.AppendLine($"            get => _{connectionName};");
        sb.AppendLine($"            set");
        sb.AppendLine($"            {{");
        sb.AppendLine($"                _{connectionName} = value;");
        sb.AppendLine($"                {propName}Editor.IsConnected = value != null;");
        sb.AppendLine($"                if (_{connectionName} != null)");
        sb.AppendLine($"                {{");
        if (editorType != "InputOnlyEditor")
            sb.AppendLine($"                    {propName}Editor.DataGetter = () => _{connectionName}.GetValue();");
        sb.AppendLine($"                    _{connectionName}.PropertyChanged += {connectionName}_PropertyChanged;");
        sb.AppendLine($"                    OnPropertyChanged();");
        sb.AppendLine($"                }}");
        sb.AppendLine($"            }}");
        sb.AppendLine($"        }}");
        sb.AppendLine($"        private void {connectionName}_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)");
        sb.AppendLine($"        {{");
        sb.AppendLine($"            {propName}Editor.PushPropertyChanged();");
        sb.AppendLine($"        }}");
        sb.AppendLine($"        public {editorType} {propName}Editor");
        sb.AppendLine($"        {{");
        sb.AppendLine($"            get");
        sb.AppendLine($"            {{");
        sb.AppendLine($"                if (_{propName}Editor == null)");
        if (editorType == "InputOnlyEditor")
            sb.AppendLine($"                    _{propName}Editor = new(this, \"{fieldName.Capitalize()}\");");
        else
            sb.AppendLine($"                    _{propName}Editor = new(this, \"{fieldName.Capitalize()}\", () => {fieldName}, x => {{ {fieldName} = x; OnPropertyChanged(\"{propName}\"); }});");
        sb.AppendLine($"                return _{propName}Editor;");
        sb.AppendLine($"            }}");
        sb.AppendLine($"        }}");
        sb.AppendLine();
    }

    private record FieldEditorInfo(INamedTypeSymbol ContainingClass, IFieldSymbol Field, ITypeSymbol EditorType);
}

public static class QuickExtensions
{
    public static string Capitalize(this string str)
    {
        return $"{char.ToUpper(str[1])}{str.Substring(2)}";
    }

    public static string Fieldify(this string str)
    {
        return $"_{char.ToLower(str[0])}{str.Substring(1)}";
    }
}