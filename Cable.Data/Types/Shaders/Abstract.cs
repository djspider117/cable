using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cable.Data.Types.Shaders;

public interface IShaderInstruction : ICableDataType
{
    bool HasDeclarations { get; }
    IEnumerable<IDeclaration> Declarations { get; }
}
public interface IVariable : IShaderInstruction;
public interface IOperand : IShaderInstruction;
public interface ITrigFunction : IShaderInstruction, IOperand;
public interface IExpression : IShaderInstruction;
public interface IOutput : IShaderInstruction;
public interface IDeclaration : IShaderInstruction
{
    IReadOnlyList<string> GetDeclarationTextx();
}

public abstract class ShaderInstructionBase : IShaderInstruction
{
    protected List<IDeclaration> _declarations = [];

    public virtual bool HasDeclarations { get; }
    public IEnumerable<IDeclaration> Declarations => GetDeclarations();

    protected virtual IEnumerable<IDeclaration> GetDeclarations() => _declarations;
}