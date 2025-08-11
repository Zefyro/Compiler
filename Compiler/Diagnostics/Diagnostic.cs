namespace Compiler.Diagnostics;

public sealed class Diagnostic(string message)
{
    public string Message { get; } = message;

    public override string ToString() => Message;
}