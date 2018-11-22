using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeneratorCLI.Logger
{
    static class Log
    {
        public static string LogFilePath { get; set; } = "";

        public static void LogMessage(string message)
        {
            Console.WriteLine(message);
            File.AppendAllLines(LogFilePath, new string[] { $"[{DateTime.Now:yyyyMMdd-HHmmss}] {message}" });
        }
    }
}
