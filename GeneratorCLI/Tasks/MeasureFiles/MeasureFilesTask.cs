using Generator.Distribution.Entropy;
using GeneratorCLI.Compression;
using GeneratorCLI.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GeneratorCLI.Tasks.Measure
{
    class MeasureFilesTask
    {
        public static void MeasureFilesInDirectory(string path, string pattern)
        {
            Log.LogFilePath = Path.Combine(path, "MeasureLog.txt");

            var logFile = new RecordsFile<MeasureFileRecord>(Path.Combine(path, "ResultFilesInfo.txt"), new AnalysisFilesLogRecordSerializer());
            var logRecords = new List<MeasureFileRecord>();

            var filesInfo = new DirectoryInfo(path).EnumerateFiles(pattern).ToList();
            int numberOfFiles = filesInfo.Count;
            int i = 0;
            foreach (var item in filesInfo)
            {
                Log.LogMessage($"Processing file [{++i}/{numberOfFiles}]: Filename {item.Name}.");

                byte[] data = File.ReadAllBytes(item.FullName);
                var logRecord = new MeasureFileRecord()
                {
                    FileName = item.Name,
                    FileSize = item.Length,
                    DataEntropy = EntropyCalculator.CalculateForData(data),
                    CompressionRatio = (double)(Zip.Compress(data).Length) / data.Length
                };
                logRecords.Add(logRecord);
            }

            logFile.WriteAllRecords(logRecords, new string[] { AnalysisFilesLogRecordSerializer.GetHeadersComment() });
        }

    }
}
