using Generator.Distribution;
using Generator.Distribution.Entropy;
using Generator.Sequence;
using GeneratorCLI.Compression;
using GeneratorCLI.Logger;
using GeneratorCLI.Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GeneratorCLI.Tasks.Generate
{
    class GenerateMarkovEntropyFilesTask
    {
        public static void GenerateFiles(string path, int initSize, int totalSize, int numberOfDifferentDependanceDepth, int numberOfDifferentEntropyValues, string fileNamePrefix = "")
        {
            Log.LogFilePath = Path.Combine(path, "GenerateLog.txt");
            Log.LogMessage($"Parameters: {{ initSize = {initSize}, totalSize = {totalSize}, dependanceDepth = {numberOfDifferentDependanceDepth}, entropyValues = {numberOfDifferentEntropyValues}, prefix = {fileNamePrefix} }}");

            var logFile = new RecordsFile<GenerateMarkovChainFileRecord>(Path.Combine(path, "FilesInfo.txt"), new GenerateMarkovChainFileRecordSerializer());
            var logRecords = new List<GenerateMarkovChainFileRecord>();

            var nameGenerator = new UniqueRandomStringGenerator(4);

            var chainGenerator = new ChainGenerator();

            Log.LogMessage($"Calculating progression values");
            double[] progressionRates = new double[numberOfDifferentEntropyValues];
            double[] entropyValues = new double[numberOfDifferentEntropyValues];
            for (int i = 0; i < numberOfDifferentEntropyValues; i++)
            {
                double targetEntropy = 8 * (1 - (double)i / (numberOfDifferentEntropyValues - 1));
                progressionRates[i] = ParameterSearch.SearchParameterValue(CalculateEntropyForProgression, targetEntropy, 1, 1000000, 0.001);
                //progressionRates[i] = Math.Pow(2, i); // Alternative way to set progression rates to pay more attention for lower values of entropy
                entropyValues[i] = CalculateEntropyForProgression(progressionRates[i]);
                Log.LogMessage($"  Progression rate {progressionRates[i]} gives entropy {entropyValues[i]}");
            }
            
            int totalItems = numberOfDifferentDependanceDepth * numberOfDifferentEntropyValues;
            int currentItemNumber = 0;
            for (int j = 0; j < numberOfDifferentDependanceDepth; j++)
            {
                int dependanceDepth = j;
                for (int i = 0; i < numberOfDifferentEntropyValues; i++)
                {
                    currentItemNumber++;

                    double progression = progressionRates[i]; // Math.Pow(2, Math.Pow((double)i / numberOfDifferentEntropyValues, 2));

                    string filename = $"{fileNamePrefix}{nameGenerator.GetUniqueString()}.data";
                    Log.LogMessage($"Generating file [{currentItemNumber}/{totalItems}]: Filename {filename}, DependanceDepth {dependanceDepth}, Progression {progression:N4}.");

                    byte[] data = chainGenerator.GenerateData(progression, initSize, totalSize, dependanceDepth);
                    byte[] compressedData = Zip.Compress(data);

                    double compressionRatio = (double)compressedData.Length / data.Length;
                    Log.LogMessage($"  Generated data entropy: {chainGenerator.TotalGeneratedEntropy}");
                    Log.LogMessage($"  Compression ratio: {compressionRatio}");

                    File.WriteAllBytes(Path.Combine(path, filename), data);

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

        private static GeometricProgressionDistribution<int> mProgressionDistribution = new GeometricProgressionDistribution<int>(Enumerable.Range(0, 256), 1);

        private static double CalculateEntropyForProgression(double progression)
        {
            mProgressionDistribution.SetProgressionRate(progression);
            return EntropyCalculator.CalculateForDistribution(mProgressionDistribution);
        }
    }

}
