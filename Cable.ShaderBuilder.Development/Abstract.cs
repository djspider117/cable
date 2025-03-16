using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cable.ShaderBuilder.Development;

public interface IShaderInstruction
{
}
public interface IVariable : IShaderInstruction;
public interface IOperand : IShaderInstruction;
public interface ITrigFunction : IShaderInstruction;
public interface IExpression : IShaderInstruction;
public interface IOutput : IShaderInstruction;
public interface IDeclaration : IShaderInstruction
{
    IReadOnlyList<string> GetDeclarations();
}
