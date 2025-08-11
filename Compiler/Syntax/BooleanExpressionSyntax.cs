namespace Compiler.Syntax;
public class BooleanExpressionSyntax(ExpressionSyntax left, SyntaxToken operationToken, ExpressionSyntax right) : ExpressionSyntax {
    public override SyntaxKind Kind => SyntaxKind.BooleanExpression;
    public ExpressionSyntax Left { get; } = left;
    public SyntaxToken OperationToken { get; } = operationToken;
    public ExpressionSyntax Right { get; } = right;
    public override IEnumerable<SyntaxNode> GetChildren() {
        yield return Left;
        yield return OperationToken;
        yield return Right;
    }
}
