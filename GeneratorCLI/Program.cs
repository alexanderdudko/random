using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeneratorCLI.Tasks;
using GeneratorCLI.Tasks.Generate;
using GeneratorCLI.Tasks.Measure;

namespace GeneratorCLI
{
    class Program
    {

        static void Main(string[] args)
        {
            var argsDict = ParseArgs(args);

            string basePath = argsDict["path"];
            var task = Enum.Parse(typeof(TasksEnum), argsDict["task"]);
            Console.WriteLine($"TASK {task}");

            switch (task)
            {
                case TasksEnum.GenerateEntropyFiles:
                    GenerateEntropyFilesTask.GenerateFiles(
                        path: Directory.CreateDirectory(Path.Combine(basePath, DateTime.Now.ToString("yyyyMMdd-HHmmss"))).FullName,
                        fileSize: int.Parse(argsDict["fileSize"]),
                        numberOfFiles: int.Parse(argsDict["numberOfFiles"]),
                        fileNamePrefix: argsDict["prefix"]
                    );
                    break;
                case TasksEnum.GenerateMarkovEntropyFiles:
                    GenerateMarkovEntropyFilesTask.GenerateFiles(
                        path: Directory.CreateDirectory(Path.Combine(basePath, DateTime.Now.ToString("yyyyMMdd-HHmmss"))).FullName,
                        initSize: int.Parse(argsDict["initSize"]),
                        totalSize: int.Parse(argsDict["fileSize"]),
                        numberOfDifferentDependanceDepth: int.Parse(argsDict["dependanceDepth"]),
                        numberOfDifferentEntropyValues: int.Parse(argsDict["entropyValues"]),
                        fileNamePrefix: argsDict["prefix"]
                    );
                    break;
                case TasksEnum.MeasureFiles:
                    MeasureFilesTask.MeasureFilesInDirectory(
                        path: basePath,
                        pattern: argsDict["pattern"]
                    );
                    break;
                case TasksEnum.GenerateQuantumRandomFile:
                    GenerateQuantumRandomFilesTask.CreateQuantumRandomFileTest(
                        path: Directory.CreateDirectory(Path.Combine(basePath, DateTime.Now.ToString("yyyyMMdd-HHmmss"))).FullName,
                        fileSize: int.Parse(argsDict["fileSize"])
                    );
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return;
        }

        private static Dictionary<string, string> GetDefaults()
        {
            var result = new Dictionary<string, string>();

            result["task"] = TasksEnum.GenerateEntropyFiles.ToString();
            result["path"] = Environment.CurrentDirectory;
            result["fileSize"] = (1024 * 1024).ToString(); // 1MB
            result["numberOfFiles"] = 200.ToString();
            result["prefix"] = "exp_";
            result["initSize"] = 32.ToString();
            result["pattern"] = "*.data";
            result["dependanceDepth"] = 6.ToString();
            result["entropyValues"] = (8 * 4 + 1).ToString();

            return result;
        }

        private static Dictionary<string, string> ParseArgs(string[] args)
        {
            var result = GetDefaults();

            foreach (var arg in args)
            {
                int index = arg.IndexOf('=');
                if (index > 0)
                    result[arg.Substring(0, index)] = arg.Substring(index + 1);
                else
                    result[arg.Substring(0, index)] = string.Empty;
            }

            return result;
        }
    }

}
