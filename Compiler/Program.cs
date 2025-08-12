using Compiler.Binding;
using Compiler.Diagnostics;
using Compiler.Syntax;

namespace Compiler;
public static class Program {
    public static readonly Dictionary<string, object> Variables = [];
    public static void Main(string[] args) {
        bool print_tree = true;
        for (;;)
        {
            Console.Write("> ");
            string? text = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Invalid Expression");
                continue;
            }
            else if (text.StartsWith("$file"))
            {
                string filePath = text[5..];
                StreamReader sr = new(filePath);
                text = sr.ReadToEnd();
                sr.Close();
                Console.WriteLine("Contents:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(text);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (text == "$cls")
            {
                Console.Clear();
                continue;
            }
            else if (text == "$tree")
            {
                print_tree = !print_tree;
                Console.WriteLine(print_tree);
                continue;
            }

            if (string.IsNullOrWhiteSpace(text))
                return;

            SyntaxTree syntaxTree = SyntaxTree.Parse(text);
            var diagnostics = new DiagnosticBag();

            ConsoleColor color = Console.ForegroundColor;
            if (print_tree)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Syntax Nodes]");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                PrettyPrintSyntaxNode(syntaxTree.Root);
                Console.ForegroundColor = color;
            }

            // Collect diagnostics from SyntaxTree parsing
            foreach (var diagnostic in syntaxTree.Diagnostics)
            {
                diagnostics.Report(diagnostic);
            }

            if (!diagnostics.ToImmutableArray().Any())
            {
                var binder = new Binder(diagnostics, Variables);
                var boundStatement = binder.BindStatement(syntaxTree.Root);
                if (print_tree)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[Bound Nodes]");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    PrettyPrintBoundNode(boundStatement);
                    Console.ForegroundColor = color;
                }
                Evaluator evaluator = new(boundStatement, diagnostics, Variables);
                object result = evaluator.Evaluate();

                if (diagnostics.ToImmutableArray().Any())
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    foreach (var diagnostic in diagnostics.ToImmutableArray())
                        Console.WriteLine(diagnostic);
                    Console.ForegroundColor = color;
                }
                else
                {
                    Console.WriteLine(result);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                foreach (var diagnostic in diagnostics.ToImmutableArray())
                    Console.WriteLine(diagnostic);
                Console.ForegroundColor = color;
            }
        }
    }
    private static void PrettyPrintSyntaxNode(SyntaxNode node, string indent = "", bool isLast = true) {
        string marker = isLast ? "└──" : "├──";

        Console.Write(indent);
        Console.Write(marker);
        Console.Write(node.Kind);

        if (node is SyntaxToken token && token.Value != null) {
            Console.Write(" ");
            Console.Write(token.Value);
        }
        Console.WriteLine();
        indent += isLast ? "   " : "│  ";
        SyntaxNode? lastChild = node.GetChildren().LastOrDefault();

        foreach (SyntaxNode child in node.GetChildren())
            PrettyPrintSyntaxNode(child, indent, child == lastChild);
    }

    private static void PrettyPrintBoundNode(BoundNode node, string indent = "", bool isLast = true) {
        string marker = isLast ? "└──" : "├──";

        Console.Write(indent);
        Console.Write(marker);
        Console.Write(node.Kind);

        if (node is BoundLiteralExpression l) {
            Console.Write(" ");
            Console.Write(l.Value);
        }
        else if (node is BoundVariableExpression v) {
            Console.Write(" ");
            Console.Write(v.VariableName);
        }
        else if (node is BoundExpression e)
        {
            Console.Write(" ");
            Console.Write(e.Type);
        }

        Console.WriteLine();
        indent += isLast ? "   " : "│  ";
        BoundNode? lastChild = node.GetChildren().LastOrDefault();

        foreach (BoundNode child in node.GetChildren())
            PrettyPrintBoundNode(child, indent, child == lastChild);
    }
}