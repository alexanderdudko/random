using System;
using System.Collections.Generic;
using System.Text;

namespace GeneratorCLI.Tasks.Measure
{
    class MeasureFileRecord
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public double DataEntropy { get; set; }
        public double CompressionRatio { get; set; }
    }

    class AnalysisFilesLogRecordSerializer : ILogFileRecordSerializer<MeasureFileRecord>
    {
        private const char SEPARATOR = '\t';

        public MeasureFileRecord Deserialize(string text)
        {
            var parts = text.Split(SEPARATOR);
            return new MeasureFileRecord()
            {
                FileName = parts[0],
                FileSize = long.Parse(parts[1]),
                DataEntropy = double.Parse(parts[2]),
                CompressionRatio = double.Parse(parts[3])
            };
        }

        public string Serialize(MeasureFileRecord record)
        {
            return string.Join(SEPARATOR,
                record.FileName,
                record.FileSize,
                record.DataEntropy,
                record.CompressionRatio
            );
        }

        public static IEnumerable<string> GetStoredFieldNames()
        {
            return new string[] 
            {
                "FileName",
                "FileSize",
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
