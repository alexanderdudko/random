using System;
using System.Collections.Generic;
using System.Text;

namespace GeneratorCLI
{
    class RandomFilesLogRecord
    {
        public string FileName { get; set; }
        public double ProgressionRate { get; set; }
        public double GeneratorEntropy { get; set; }
        public double DataEntropy { get; set; }
    }

    class RandomFilesLogRecordSerializer : ILogFileRecordSerializer<RandomFilesLogRecord>
    {
        private const char SEPARATOR = '\t';

        public RandomFilesLogRecord Deserialize(string text)
        {
            var parts = text.Split(SEPARATOR);
            return new RandomFilesLogRecord()
            {
                FileName = parts[0],
                ProgressionRate = double.Parse(parts[1]),
                GeneratorEntropy = double.Parse(parts[2]),
                DataEntropy = double.Parse(parts[3])
            };
        }

        public string Serialize(RandomFilesLogRecord record)
        {
            return string.Join(SEPARATOR,
                record.FileName,
                record.ProgressionRate,
                record.GeneratorEntropy,
                record.DataEntropy
            );
        }
    }
}
