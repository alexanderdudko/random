using Generator.Distribution;
using Generator.Distribution.Entropy;
using Generator.RandomGenerator.Continuous;
using Generator.RandomGenerator.Discrete;
using Generator.ResourceAccess;
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
    class GenerateQuantumRandomFilesTask
    {
        public static void CreateQuantumRandomFileTest(string path, int fileSize)
        {
            Log.LogFilePath = Path.Combine(path, "QuantumLog.txt");

            var uniformRandomSource = new WebRandomSource(new LargeDataQuantumRngAnuEduAccessDetails());

            byte[] bytes = uniformRandomSource.GetBytesSource().Take(fileSize).ToArray();
            File.WriteAllBytes(Path.Combine(path, "quantum.data"), bytes);

            byte[] compressedData = Zip.Compress(bytes);
            File.WriteAllBytes(Path.Combine(path, "compressed.data"), compressedData);
        }
    }
}
