using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GeneratorCLI
{
    class LogFile<TRecord>
    {
        private const string COMMENT_PREFIX = "#";

        private string mPath;
        private ILogFileRecordSerializer<TRecord> mSerializer;

        public LogFile(string path, ILogFileRecordSerializer<TRecord> serializer)
        {
            mPath = path;
            mSerializer = serializer;
        }

        public void WriteAllRecords(IEnumerable<TRecord> records, IEnumerable<string> comments = null)
        {
            var fileData = records.Select(r => mSerializer.Serialize(r));
            if (comments != null)
                fileData = comments.Select(c => COMMENT_PREFIX + " " + c).Union(fileData);
            File.WriteAllLines(mPath, fileData);
        }

        public IEnumerable<TRecord> ReadAllRecords()
        {
            var fileData = File.ReadAllLines(mPath);
            return fileData.Where(t => !t.StartsWith(COMMENT_PREFIX)).Select(t => mSerializer.Deserialize(t));
        }
    }

    interface ILogFileRecordSerializer<TRecord>
    {
        string Serialize(TRecord record);
        TRecord Deserialize(string text);
    }
}
