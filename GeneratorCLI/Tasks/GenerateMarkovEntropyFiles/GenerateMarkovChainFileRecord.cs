using System;
using System.Collections.Generic;
using System.Text;

namespace GeneratorCLI.Tasks.Generate
{
    class GenerateMarkovChainFileRecord
    {
        public string FileName { get; set; }
        public int DependanceDepth { get; set; }
        public double ProgressionRate { get; set; }
        public double GeneratorEntropy { get; set; }
        public double DataEntropy { get; set; }
        public double CompressionRatio { get; set; }
    }

    class GenerateMarkovChainFileRecordSerializer : ILogFileRecordSerializer<GenerateMarkovChainFileRecord>
    {
        private const char SEPARATOR = '\t';

        public GenerateMarkovChainFileRecord Deserialize(string text)
        {
            var parts = text.Split(SEPARATOR);
            return new GenerateMarkovChainFileRecord()
            {
                FileName = parts[0],
                DependanceDepth = int.Parse(parts[1]),
                ProgressionRate = double.Parse(parts[2]),
                GeneratorEntropy = double.Parse(parts[3]),
                DataEntropy = double.Parse(parts[4]),
                CompressionRatio = double.Parse(parts[5]),
            };
        }

        public string Serialize(GenerateMarkovChainFileRecord record)
        {
            return string.Join(SEPARATOR,
                record.FileName,
                record.DependanceDepth,
                record.ProgressionRate,
                record.GeneratorEntropy,
                record.DataEntropy,
                record.CompressionRatio
            );
        }

        public static IEnumerable<string> GetStoredFieldNames()
        {
            return new string[] 
            {
                "FileName",
                "DependanceDepth",
                "ProgressionRate",
                "GeneratorEntropy",
                "DataEntropy",
                "CompressionRatio"
            };
        }

        public static string GetHeadersComment()
        {
            return string.Join(SEPARATOR, GetStoredFieldNames());
        }
    }
}
