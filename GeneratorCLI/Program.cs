using System;
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
            string basePath = @"H:\projects\data\random";

            var task = TasksEnum.GenerateEntropyFiles; //TODO: Can be retrieved from arguments

            switch (task)
            {
                case TasksEnum.GenerateEntropyFiles:
                    GenerateEntropyFilesTask.GenerateFiles(
                        path: Directory.CreateDirectory(Path.Combine(basePath, DateTime.Now.ToString("yyyyMMdd-HHmmss"))).FullName,
                        fileSize: 1024 * 1024, // 1 MB
                        numberOfFiles: 200,
                        fileNamePrefix: "exp1_"
                    );
                    break;
                case TasksEnum.GenerateMarkovEntropyFiles:
                    GenerateMarkovEntropyFilesTask.GenerateFiles(
                        path: Directory.CreateDirectory(Path.Combine(basePath, DateTime.Now.ToString("yyyyMMdd-HHmmss"))).FullName,
                        initSize: 32,
                        totalSize: 1024 * 1024, // 1 MB
                        numberOfDifferentDependanceDepth: 15,
                        numberOfDifferentEntropyValues: 15,
                        fileNamePrefix: "exp2_"
                    );
                    break;
                case TasksEnum.MeasureFiles:
                    MeasureFilesTask.MeasureFilesInDirectory(
                        path: new DirectoryInfo(Path.Combine(basePath, @"20181113-104802 - Upload 1\result_files_copy\result_files")).FullName,
                        pattern: "*.zcaps"
                    );
                    break;
                case TasksEnum.GenerateQuantumRandomFile:
                    GenerateQuantumRandomFilesTask.CreateQuantumRandomFileTest(
                        path: Directory.CreateDirectory(Path.Combine(basePath, DateTime.Now.ToString("yyyyMMdd-HHmmss"))).FullName,
                        fileSize: 1024 * 1024 // 1 MB
                    );
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return;
        }


    }

}
