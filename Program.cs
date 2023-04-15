using System;

namespace LoxInterperter1 // Note: actual namespace depends on the project name.
{
    internal partial class Program
    {
        private static bool hadError;

        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                System.Console.WriteLine("Usage: jlox [script]"); ;
                Environment.Exit(64);
            }
            else if (args.Length == 1)
            {
                RunFile(args[0]);
            }
            else
            {
                RunPrompt();
            }
        }

        private static void RunPrompt()
        {
            for (; ; )
            {
                System.Console.WriteLine("> ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }
                Run(line);
                hadError = false;
            }
        }

        private static void Run(string source)
        {
            var scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();
            foreach (var token in tokens)
            {
                System.Console.WriteLine(token);
            }
        }

        internal static void Error(int line, string message)
        {
            Report(line, string.Empty, message);
        }

        private static void Report(int line, string where, string message)
        {
            System.Console.Error.WriteLine($"[line {line} Error {where}: {message}]");
            hadError = true;
        }

        private static void RunFile(string path)
        {
            var fileContent = File.ReadAllText(path);
            Run(fileContent);
            if (hadError)
            {
                Environment.Exit(65);
            }
        }
    }
}