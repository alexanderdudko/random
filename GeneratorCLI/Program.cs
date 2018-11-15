﻿using Generator;
using Generator.Distribution;
using Generator.Distribution.Entropy;
using Generator.RandomGenerator.Discrete;
using Generator.RandomGenerator.Continuous;
using Generator.ResourceAccess;
using Generator.Sequence;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace GeneratorCLI
{
    class Program
    {
        static string mLogFilePath;

        static void Main(string[] args)
        {
            string basePath = @"H:\projects\data\random";
            var path = Directory.CreateDirectory(Path.Combine(basePath, DateTime.Now.ToString("yyyyMMdd-HHmmss"))).FullName;
            mLogFilePath = Path.Combine(path, "log.txt");

            int fileSize = 1024 * 1024; // 1MB
            int numberOfFiles = 200;

            GenerateFiles(path, fileSize, numberOfFiles, "exp1_");
        }

        private static void GenerateFiles(string path, int fileSize, int numberOfFiles, string fileNamePrefix = "")
        {
            var logFile = new LogFile<RandomFilesLogRecord>(Path.Combine(path, "FilesInfo.txt"), new RandomFilesLogRecordSerializer());
            var logRecords = new List<RandomFilesLogRecord>();

            var nameGenerator = new UniqueRandomStringGenerator(4);

            int n = 256;
            var distribution = new GeometricProgressionDistribution<byte>(Enumerable.Range(0, n).Select(x => (byte)x), 1);
            double k = 1;
            for (int i = 0; i < numberOfFiles; i++)
            {
                double progression = Math.Pow(k, 1.0 / (n - 1));
                distribution.SetProgressionRate(progression);

                string filename = $"{fileNamePrefix}{nameGenerator.GetUniqueString()}.data";
                LogMessage($"Generating file [{i + 1}/{numberOfFiles}]: Filename {filename}.");
                byte[] data = GenerateData(distribution, fileSize);
                byte[] compressedData = Compress(data);

                double compressionRatio = (double)compressedData.Length / data.Length;
                LogMessage($"  Compression ratio: {compressionRatio}");
                File.WriteAllBytes(Path.Combine(path, filename), data);
                var logRecord = new RandomFilesLogRecord()
                {
                    FileName = filename,
                    ProgressionRate = progression,
                    GeneratorEntropy = EntropyCalculator.CalculateForDistribution(distribution),
                    DataEntropy = EntropyCalculator.CalculateForData(data),
                    CompressionRatio = compressionRatio,
                    CompressedDataEntropy = EntropyCalculator.CalculateForData(compressedData)
                };
                logRecords.Add(logRecord);

                k *= 2;
            }

            logFile.WriteAllRecords(logRecords, new string[] { RandomFilesLogRecordSerializer.GetHeadersComment() });
        }

        private static byte[] Compress(byte[] data)
        {
            using (var ms = new MemoryStream())
            using (var zip = new GZipStream(ms, CompressionLevel.Optimal))
            {
                zip.Write(data);
                return ms.ToArray();
            }
        }

        private static byte[] GenerateData(IDiscreteValueDistribution<byte> distribution, int size, double entropyDiff = 0.001)
        {
            double generatorEntropy = EntropyCalculator.CalculateForDistribution(distribution);
            LogMessage($"  Generator entropy {generatorEntropy:N5}");

            //var uniformRandomSource = new SystemUniformRandomSource();
            //var uniformRandomSource = new CryptoUniformRandomSource();
            //var uniformRandomSource = new WebRandomSource(new RandomOrgAccessDetails());
            var uniformRandomSource = new WebRandomSource(new LargeDataQuantumRngAnuEduAccessDetails());
            var randomSource = new DiscreteFiniteSetValueGenerator<byte>(distribution, uniformRandomSource);
            var sg = new SequenceGenerator<byte>(randomSource);
            byte[] bytes;
            double empiricalDataEntropy = 0;
            double difference;
            do
            {
                bytes = sg.Generate(size).ToArray();

                empiricalDataEntropy = EntropyCalculator.CalculateForData(bytes);
                difference = Math.Abs(generatorEntropy - empiricalDataEntropy);
                LogMessage($"  Generated empirical data with entropy {empiricalDataEntropy:N5}. Difference: {difference:N5}.");
            } while (difference > entropyDiff);

            return bytes;
        }

        private static void LogMessage(string message)
        {
            Console.WriteLine(message);
            File.AppendAllLines(mLogFilePath, new string[] { $"[{DateTime.Now:yyyyMMdd-HHmmss}] {message}" });
        }

    }

}
