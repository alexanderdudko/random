using System;
using System.Collections.Generic;
using System.Text;

namespace GeneratorCLI.Tasks.Generate
{
    class GenerateFileRecord
    {
        public string FileName { get; set; }
        public double ProgressionRate { get; set; }
        public double GeneratorEntropy { get; set; }
        public double DataEntropy { get; set; }
        public double CompressionRatio { get; set; }
        public double CompressedDataEntropy { get; set; }
    }

    class RandomFilesLogRecordSerializer : ILogFileRecordSerializer<GenerateFileRecord>
    {
        private const char SEPARATOR = '\t';

        public GenerateFileRecord Deserialize(string text)
        {
            var parts = text.Split(SEPARATOR);
            return new GenerateFileRecord()
            {
                FileName = parts[0],
                ProgressionRate = double.Parse(parts[1]),
                GeneratorEntropy = double.Parse(parts[2]),
                DataEntropy = double.Parse(parts[3]),
                CompressionRatio = double.Parse(parts[4]),
                CompressedDataEntropy = double.Parse(parts[5])
            };
        }

        public string Serialize(GenerateFileRecord record)
        {
            return string.Join(SEPARATOR,
                record.FileName,
                record.ProgressionRate,
                record.GeneratorEntropy,
                record.DataEntropy,
                record.CompressionRatio,
                record.CompressedDataEntropy
            );
        }

        public static IEnumerable<string> GetStoredFieldNames()
        {
            return new string[] 
            {
                "FileName",
                "ProgressionRate",
                "GeneratorEntropy",
                "DataEntropy",
                "CompressionRatio",
                "CompressedDataEntropy"
            };
        }

        public static string GetHeadersComment()
        {
            return string.Join(SEPARATOR, GetStoredFieldNames());
        }
    }
}
