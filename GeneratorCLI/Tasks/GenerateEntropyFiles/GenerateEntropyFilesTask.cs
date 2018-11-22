using Generator.Distribution;
using Generator.Distribution.Entropy;
using Generator.RandomGenerator.Continuous;
using Generator.RandomGenerator.Discrete;
using Generator.Sequence;
using GeneratorCLI.Compression;
using GeneratorCLI.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GeneratorCLI.Tasks.Generate
{
    class GenerateEntropyFilesTask
    {
        public static void GenerateFiles(string path, int fileSize, int numberOfFiles, string fileNamePrefix = "")
        {
            Log.LogFilePath = Path.Combine(path, "GenerateLog.txt");


            var logFile = new RecordsFile<GenerateFileRecord>(Path.Combine(path, "FilesInfo.txt"), new RandomFilesLogRecordSerializer());
            var logRecords = new List<GenerateFileRecord>();

            var nameGenerator = new UniqueRandomStringGenerator(4);

            int n = 256;
            var distribution = new GeometricProgressionDistribution<byte>(Enumerable.Range(0, n).Select(x => (byte)x), 1);

            for (int i = 0; i < numberOfFiles; i++)
            {
                double progression = Math.Pow(2, Math.Pow((double)i / numberOfFiles, 2));
                distribution.SetProgressionRate(progression);

                string filename = $"{fileNamePrefix}{nameGenerator.GetUniqueString()}.data";
                Log.LogMessage($"Generating file [{i + 1}/{numberOfFiles}]: Filename {filename}.");
                byte[] data = GenerateData(distribution, fileSize, entropyDiff: 1);
                byte[] compressedData = Zip.Compress(data);

                double compressionRatio = (double)compressedData.Length / data.Length;
                Log.LogMessage($"  Compression ratio: {compressionRatio}");

                File.WriteAllBytes(Path.Combine(path, filename), data);

                var logRecord = new GenerateFileRecord()
                {
                    FileName = filename,
                    ProgressionRate = progression,
                    GeneratorEntropy = EntropyCalculator.CalculateForDistribution(distribution),
                    DataEntropy = EntropyCalculator.CalculateForData(data),
                    CompressionRatio = compressionRatio,
                    CompressedDataEntropy = EntropyCalculator.CalculateForData(compressedData)
                };
                logRecords.Add(logRecord);
            }

            logFile.WriteAllRecords(logRecords, new string[] { RandomFilesLogRecordSerializer.GetHeadersComment() });
        }

        private static byte[] GenerateData(IDiscreteValueDistribution<byte> distribution, int size, double entropyDiff = 0.001)
        {
            double generatorEntropy = EntropyCalculator.CalculateForDistribution(distribution);
            Log.LogMessage($"  Generator entropy {generatorEntropy:N5}");

            //var uniformRandomSource = new SystemUniformRandomSource();
            var uniformRandomSource = new CryptoUniformRandomSource();
            //var uniformRandomSource = new WebRandomSource(new RandomOrgAccessDetails());
            //var uniformRandomSource = new WebRandomSource(new LargeDataQuantumRngAnuEduAccessDetails());
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
                Log.LogMessage($"  Generated empirical data with entropy {empiricalDataEntropy:N5}. Difference: {difference:N5}.");
            } while (difference > entropyDiff);

            return bytes;
        }
    }
}
