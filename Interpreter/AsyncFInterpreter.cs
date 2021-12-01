using System;
using System.IO;

using Components;
using Components.Ast;
using Components.Exceptions;

namespace Interpreter
{
    /// <summary>
    /// Basic console interpreter
    /// </summary>
    public class AsyncFInterpreter
    {
        /// <summary>
        /// Parse and execute AsyncF programs
        /// </summary>
        /// <param name="args">Source file paths</param>
        public static void Main(string[] args)
        {
            Program globalFunctionsTable = new();

            Parser parser = new(new Lexer(""));

            foreach (string arg in args)
            {
                if (arg.EndsWith(".asyncf"))
                {
                    try
                    {
                        string path = File.Exists(arg) ? arg : Directory.GetCurrentDirectory() + '\\' + arg;
                        if (!File.Exists(path)) throw new Exception($"File {path} does not exist.");

                        using StreamReader reader = new(path);
                        parser.Reset(reader.ReadToEnd());
                        globalFunctionsTable.AddFunctionsTable(parser.Parse().GetAllFunctions());
                    }
                    catch (AsyncFException e)
                    {
                        Console.WriteLine($"{e.Message} at file {arg}");
                        return;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
                }
            }

            if (globalFunctionsTable.GetFunction("main") != null) Run(globalFunctionsTable);
            else Console.WriteLine("Program doesnt contain 'main' functions, execution stopped.");
        }


        /// <summary>
        /// Execute program
        /// </summary>
        /// <param name="prog">Program AST</param>
        public static async void Run(Program prog)
        {
            try
            {
                Evaluator.SetEvaluateFunctionsTable(prog.GetAllFunctions());
                await Evaluator.Evaluate(prog.GetFunction("main").Body);
            }
            catch (AsyncFException e)
            {
                Console.WriteLine($"{e.Message}\nExecution stopped.");
            }
        }

    }
}

