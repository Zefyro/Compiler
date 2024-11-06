using Lexer;
using Parser;

namespace Compiler;
public static class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("> ");
			string? text = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(text))
			{
				Console.WriteLine("Invalid Expression");
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
			
			if (string.IsNullOrWhiteSpace(text))
				return;
			
			SyntaxTree syntaxTree = SyntaxTree.Parse(text);
			
			ConsoleColor color = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkGray;
			PrettyPrint(syntaxTree.Root);
			Console.ForegroundColor = color;

			if (!syntaxTree.Diagnostics.Any())
			{
				Evaluator.Evaluator evaluator = new(syntaxTree.Root);
				object result = evaluator.Evaluate();
				Console.WriteLine(result);
			}
			else 
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
				foreach (string diagnostic in syntaxTree.Diagnostics)
					Console.WriteLine(diagnostic);
				Console.ForegroundColor = color;
			}
        }
    }
	private static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
	{
		string marker = isLast ? "└──" : "├──";

		Console.Write(indent);
		Console.Write(marker);
		Console.Write(node.Kind);

		if (node is SyntaxToken token && token.Value != null)
		{
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