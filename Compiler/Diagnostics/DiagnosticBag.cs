using System.Collections.Immutable;

namespace Compiler.Diagnostics;

public sealed class DiagnosticBag
{
    private List<Diagnostic> _diagnostics = [];

    public ImmutableArray<Diagnostic> ToImmutableArray() => [.. _diagnostics];

    public void Report(string message)
    {
        _diagnostics.Add(new Diagnostic(message));
    }

    public void ReportUndefinedVariable(string variableName)
    {
        Report($"Variable '{variableName}' is not defined.");
    }

    public void ReportInvalidOperator(string operatorName, Type? leftType, Type? rightType)
    {
        Report($"Operator '{operatorName}' is not defined for types '{leftType}' and '{rightType}'.");
    }
}