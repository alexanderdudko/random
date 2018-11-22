using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace GeneratorCLI.Compression
{
    public static class Zip
    {
        public static byte[] Compress(byte[] data)
        {
            using (var msTo = new MemoryStream())
            {
                using (var msFrom = new MemoryStream(data))
                using (var zip = new GZipStream(msTo, CompressionMode.Compress))
                {
                    msFrom.CopyTo(zip);
                }
                return msTo.ToArray();
            }
        }

        public static byte[] Decompress(byte[] data)
        {
            using (var msTo = new MemoryStream())
            {
                using (var msFrom = new MemoryStream(data))
                using (var zip = new GZipStream(msFrom, CompressionMode.Decompress))
                {
                    zip.CopyTo(msTo);
                }
                return msTo.ToArray();
            }
        }

    }
}
