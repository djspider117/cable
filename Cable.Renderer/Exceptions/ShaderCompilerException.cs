namespace Cable.Renderer.Exceptions;

[Serializable]
public class ShaderCompilerException : Exception
{
    public ShaderCompilerException() { }
    public ShaderCompilerException(string message) : base(message) { }
    public ShaderCompilerException(string message, Exception inner) : base(message, inner) { }
}
