namespace Compiler;
public sealed class Binder {
    public Dictionary<string, object> Variables = [];
    public void BindExpression(AssignmentExpressionSyntax exprSyntax) {
        Variables.TryGetValue(exprSyntax.VariableToken.Text!, out object? value);
        object expression = new Evaluator(exprSyntax.Expression).Evaluate();
        if (value is null) {
            Variables.Add(exprSyntax.VariableToken.Text!, expression);
            return;
        }
        Variables[exprSyntax.VariableToken.Text!] = expression;
    }
}