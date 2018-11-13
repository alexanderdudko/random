using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GeneratorCLI
{
    class LogFile<TRecord>
    {
        private string mPath;
        private ILogFileRecordSerializer<TRecord> mSerializer;

        public LogFile(string path, ILogFileRecordSerializer<TRecord> serializer)
        {
            mPath = path;
            mSerializer = serializer;
        }

        public void WriteAllRecords(IEnumerable<TRecord> records)
        {
            File.WriteAllLines(mPath, records.Select(r => mSerializer.Serialize(r)));
        }

        public IEnumerable<TRecord> ReadAllRecords()
        {
            return File.ReadAllLines(mPath).Select(t => mSerializer.Deserialize(t));
        }
    }

    interface ILogFileRecordSerializer<TRecord>
    {
        string Serialize(TRecord record);
        TRecord Deserialize(string text);
    }
}
