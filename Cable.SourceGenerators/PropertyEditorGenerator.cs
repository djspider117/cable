using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
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

        context.RegisterSourceOutput(classDeclarations, (spc, classes) =>
        {
            foreach (var classSymbol in classes)
            {
                var className = classSymbol.Name;
                var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
                var fields = classSymbol.GetMembers().OfType<IFieldSymbol>()
                    .Where(field => field.GetAttributes().Any(a => a.AttributeClass?.Name == "PropertyEditorAttribute"))
                    .ToList();

                if (fields.Count == 0)
                    continue;

                var generatedCode = GeneratePropertyEditorMethod(className, namespaceName, fields);
                spc.AddSource($"{className}.g.cs", SourceText.From(generatedCode, Encoding.UTF8));
            }
        });
    }

    private bool FilterSymbols(SyntaxNode node, CancellationToken ctok)
    {
        if (node is not ClassDeclarationSyntax cdecl)
            return false;

        var isNodeData = cdecl?.AttributeLists.SelectMany(x => x.Attributes)?.Any(x => x.Name?.ToString()?.Contains("NodeData") ?? false) ?? false;
        var shouldExclude = cdecl?.AttributeLists.SelectMany(x => x.Attributes)?.Any(x => x.Name?.ToString()?.Contains("ExcludeFromSourceGeneration") ?? false) ?? false;

        return !shouldExclude && isNodeData;
    }

    private static string GeneratePropertyEditorMethod(string className, string namespaceName, List<IFieldSymbol> fields)
    {
        var sb = new StringBuilder();
        sb.AppendLine("using Cable.App.Extensions;");
        sb.AppendLine("using Cable.App.Models.Data;");
        sb.AppendLine("using Cable.App.Models.Data.Connections;");
        sb.AppendLine("using Cable.App.ViewModels.Data.PropertyEditors;");
        sb.AppendLine();
        sb.AppendLine($"namespace {namespaceName}");
        sb.AppendLine("{");
        sb.AppendLine($"    public partial class {className} : NodeDataBase");
        sb.AppendLine("    {");
        sb.AppendLine();

        foreach (var field in fields)
        {
            var fieldName = field.Name;
            var propName = field.Name.Capitalize();
            var editorType = field.GetAttributes()
                .FirstOrDefault(a => a.AttributeClass?.Name == "PropertyEditorAttribute")
                ?.AttributeClass?.TypeArguments.FirstOrDefault()?.ToDisplayString() ?? "UnknownEditor";

            var connectionName = $"{propName}Connection";
            sb.AppendLine($"        private IConnection<{field.Type.ToDisplayString()}>? _{connectionName};");
            sb.AppendLine($"        private {editorType}? _{propName}Editor;");
            sb.AppendLine($"        public {field.Type.ToDisplayString()} {propName} => {connectionName} == null ? {fieldName} : {connectionName}.GetValue();");
            sb.AppendLine($"        public IConnection<{field.Type.ToDisplayString()}>? {connectionName}");
            sb.AppendLine($"        {{");
            sb.AppendLine($"            get => _{connectionName};");
            sb.AppendLine($"            set");
            sb.AppendLine($"            {{");
            sb.AppendLine($"                _{connectionName} = value;");
            sb.AppendLine($"                {propName}Editor.IsConnected = value != null;");
            sb.AppendLine($"                if (_{connectionName} != null)");
            sb.AppendLine($"                {{");
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
            sb.AppendLine($"                    _{propName}Editor = new(this, \"{fieldName.Capitalize()}\", () => {fieldName}, x => {{ {fieldName} = x; OnPropertyChanged(\"{propName}\"); }});");
            sb.AppendLine($"                return _{propName}Editor;");
            sb.AppendLine($"            }}");
            sb.AppendLine($"        }}");
            sb.AppendLine();
        }

        sb.AppendLine();
        sb.AppendLine("        public override IEnumerable<IPropertyEditor> GetPropertyEditors()");
        sb.AppendLine("        {");

        foreach (var field in fields)
        {
            var fieldName = field.Name;
            var propName = field.Name.Capitalize();
            sb.AppendLine($"            yield return {propName}Editor;");
        }

        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }


    private record FieldEditorInfo(INamedTypeSymbol ContainingClass, IFieldSymbol Field, ITypeSymbol EditorType);
}

public static class QuickExtensions
{
    public static string Capitalize(this string str)
    {
        return $"{char.ToUpper(str[1])}{str.Substring(2)}";
    }
}