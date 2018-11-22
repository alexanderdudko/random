using GeneratorCLI.Compression;
using System;
using System.Text;
using Xunit;

namespace GeneratorCLIUnitTest
{
    public class CompressionTests
    {
        [Fact]
        public void CompressDecompressTest()
        {
            string data = "some valuable message";
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] compressed = Zip.Compress(bytes);
            byte[] decompressed = Zip.Decompress(compressed);
            string decompressedData = Encoding.UTF8.GetString(decompressed);

            Assert.Equal(bytes.Length, decompressed.Length);
            Assert.Equal(bytes, decompressed);
            Assert.Equal(data, decompressedData);
        }

    }
}
