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


            var logFile = new RecordsFile<GenerateFileRecord>(Path.Combine(path, "FilesInfo.txt"), new RandomFilesLogRecordSerializer());
            var logRecords = new List<GenerateFileRecord>();

            var nameGenerator = new UniqueRandomStringGenerator(4);

            var chainGenerator = new ChainGenerator();

            for (int i = 0; i < numberOfDifferentEntropyValues; i++)
            {
                double progression = Math.Pow(2, Math.Pow((double)i / numberOfDifferentEntropyValues, 2));

                for (int j = 0; j < numberOfDifferentDependanceDepth; j++)
                {
                    int dependanceDepth = j;

                    string filename = $"{fileNamePrefix}{nameGenerator.GetUniqueString()}.data";
                    Log.LogMessage($"Generating file [{i + 1}/{numberOfDifferentEntropyValues}]: Filename {filename}.");

                    byte[] data = chainGenerator.GenerateData(progression, initSize, totalSize, dependanceDepth);
                    byte[] compressedData = Zip.Compress(data);

                    double compressionRatio = (double)compressedData.Length / data.Length;
                    Log.LogMessage($"  Compression ratio: {compressionRatio}");

                    File.WriteAllBytes(Path.Combine(path, filename), data);

                    var logRecord = new GenerateFileRecord()
                    {
                        FileName = filename,
                        ProgressionRate = progression,
                        GeneratorEntropy = chainGenerator.TotalGeneratedEntropy,
                        DataEntropy = EntropyCalculator.CalculateForData(data),
                        CompressionRatio = compressionRatio,
                        CompressedDataEntropy = EntropyCalculator.CalculateForData(compressedData)
                    };
                    logRecords.Add(logRecord);
                }
            }

            logFile.WriteAllRecords(logRecords, new string[] { RandomFilesLogRecordSerializer.GetHeadersComment() });
        }

    }

}
