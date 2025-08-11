using Compiler.Syntax;

namespace Compiler;
public sealed class Binder {
    public Dictionary<string, object> Variables = [];
    public readonly Dictionary<string, double> Constants = new() {
        {"PHI", (1 + Math.Sqrt(5)) / 2},
        {"PI", Math.PI},
        {"TAU", Math.Tau},
    };
    public void BindExpression(AssignmentExpressionSyntax exprSyntax) {
        object expression = new Evaluator(exprSyntax.Expression).Evaluate();
        if (Variables.ContainsKey(exprSyntax.VariableToken.Text!)) {
            Variables[exprSyntax.VariableToken.Text!] = expression;
            return;
        }
        Variables.Add(exprSyntax.VariableToken.Text!, expression);
    }
}