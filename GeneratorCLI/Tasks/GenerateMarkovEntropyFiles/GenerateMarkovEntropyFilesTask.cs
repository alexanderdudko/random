using Generator.Distribution.Entropy;
using Generator.Sequence;
using GeneratorCLI.Compression;
using GeneratorCLI.Logger;
using System;
using System.Collections.Generic;
using System.IO;

namespace GeneratorCLI.Tasks.Generate
{
    class GenerateMarkovEntropyFilesTask
    {
        public static void GenerateFiles(string path, int initSize, int totalSize, int numberOfDifferentDependanceDepth, int numberOfDifferentEntropyValues, string fileNamePrefix = "")
        {
            Log.LogFilePath = Path.Combine(path, "GenerateLog.txt");


            var logFile = new RecordsFile<GenerateMarkovChainFileRecord>(Path.Combine(path, "FilesInfo.txt"), new GenerateMarkovChainFileRecordSerializer());
            var logRecords = new List<GenerateMarkovChainFileRecord>();

            var nameGenerator = new UniqueRandomStringGenerator(4);

            var chainGenerator = new ChainGenerator();

            int totalItems = numberOfDifferentDependanceDepth * numberOfDifferentEntropyValues;
            int currentItemNumber = 0;
            for (int j = 0; j < numberOfDifferentDependanceDepth; j++)
            {
                int dependanceDepth = j;
                for (int i = 0; i < numberOfDifferentEntropyValues; i++)
                {
                    currentItemNumber++;

                    double progression = Math.Pow(2, Math.Pow((double)i / numberOfDifferentEntropyValues, 2));

                    string filename = $"{fileNamePrefix}{nameGenerator.GetUniqueString()}.data";
                    Log.LogMessage($"Generating file [{currentItemNumber}/{totalItems}]: Filename {filename}, DependanceDepth {dependanceDepth}, Progression {progression:N4}.");

                    byte[] data = chainGenerator.GenerateData(progression, initSize, totalSize, dependanceDepth);
                    byte[] compressedData = Zip.Compress(data);

                    double compressionRatio = (double)compressedData.Length / data.Length;
                    Log.LogMessage($"  Generated data entropy: {chainGenerator.TotalGeneratedEntropy}");
                    Log.LogMessage($"  Compression ratio: {compressionRatio}");

                    //File.WriteAllBytes(Path.Combine(path, filename), data);

                    var logRecord = new GenerateMarkovChainFileRecord()
                    {
                        FileName = filename,
                        DependanceDepth = dependanceDepth,
                        ProgressionRate = progression,
                        GeneratorEntropy = chainGenerator.TotalGeneratedEntropy,
                        DataEntropy = EntropyCalculator.CalculateForData(data),
                        CompressionRatio = compressionRatio
                    };
                    logRecords.Add(logRecord);
                }
            }

            logFile.WriteAllRecords(logRecords, new string[] { GenerateMarkovChainFileRecordSerializer.GetHeadersComment() });
        }
    }

}
