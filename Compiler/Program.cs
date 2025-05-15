namespace Compiler;
public static class Program {
    public static readonly Binder Binder = new();
    public static List<string> Diagnostics = [];
    public static void Main(string[] args) {
        bool print_tree = true;
        for (;;) {
            Console.Write("> ");
            string? text = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(text)) {
                Console.WriteLine("Invalid Expression");
            }
            else if (text.StartsWith("$file")) {
                string filePath = text[5..];
                StreamReader sr = new(filePath);
                text = sr.ReadToEnd();
                sr.Close();
                Console.WriteLine("Contents:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(text);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (text == "$cls") {
                Console.Clear();
                continue;
            }
            else if (text == "$print_vars") {
                Console.WriteLine("Constants");
                foreach (var kvp in Binder.Constants)
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                if (Binder.Variables.Count != 0)
                    Console.WriteLine("Variables");
                foreach (var kvp in Binder.Variables)
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                
                continue;
            }
            else if (text == "$help") {
                Console.WriteLine(
                    "Operators: '+', '-', '*', '/', '(', ')', '**', '=', '==', '<', '>', '<=','>=', ';'\n" +
                    "Comments: '// single line', '/* multi line */'" +
                    "$help - show this help message\n" + 
                    "$cls - clear console\n$print_vars - print used variable names and their values\n" + 
                    "$file <path> - evaluate the contents of a file\n" + 
                    "$tree - toggle syntax tree"
                );
                continue;
            }
            else if (text == "$tree") {
                print_tree = !print_tree;
                Console.WriteLine(print_tree);
                continue;
            }
            
            if (string.IsNullOrWhiteSpace(text))
                return;
            
            SyntaxTree syntaxTree = SyntaxTree.Parse(text);
            
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            if (print_tree)
                PrettyPrint(syntaxTree.Root);
            Console.ForegroundColor = color;

            if (!syntaxTree.Diagnostics.Any()) {
                Evaluator evaluator = new(syntaxTree.Root);
                object result = evaluator.Evaluate();

                if (Diagnostics.Count != 0) {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    foreach (string diagnostic in Diagnostics)
                        Console.WriteLine(diagnostic);
                    Console.ForegroundColor = color;
                    Diagnostics = [];
                }
                Console.WriteLine(result);
            }
            else  {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                foreach (string diagnostic in syntaxTree.Diagnostics)
                    Console.WriteLine(diagnostic);
                Console.ForegroundColor = color;
            }
        }
    }
    private static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true) {
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
            PrettyPrint(child, indent, child == lastChild);
    }
}