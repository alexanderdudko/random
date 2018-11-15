using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Generator.ResourceAccess
{
    public class QuantumRngAnuEduAccessDetails : IWebResourceAccessDetails
    {
        // https://qrng.anu.edu.au/API/api-demo.php

        private const int NUMBERS_COUNT = 1024;

        public string RequestUrl => "https://qrng.anu.edu.au/API/jsonI.php?length=1024&type=uint8";

        public byte[] ProcessResponseToNumbers(Stream response)
        {
            using (var sr = new StreamReader(response))
            using (var jr = new JsonTextReader(sr))
            {
                JObject j = JObject.Load(jr);
                return j.GetValue("data").Values<byte>().ToArray();
            }
        }
    }

    public class LargeDataQuantumRngAnuEduAccessDetails : IWebResourceAccessDetails
    {
        // https://qrng.anu.edu.au/API/api-demo.php

        private const int BLOCKS_COUNT = 1024;
        private const int BYTES_IN_BLOCK = 1024;

        public string RequestUrl => "https://qrng.anu.edu.au/API/jsonI.php?length=1024&type=hex16&size=1024";

        public byte[] ProcessResponseToNumbers(Stream response)
        {
            byte[] result = new byte[BLOCKS_COUNT * BYTES_IN_BLOCK];
            string[] hexArray;
            using (var sr = new StreamReader(response))
            using (var jr = new JsonTextReader(sr))
            {
                JObject j = JObject.Load(jr);
                hexArray = j.GetValue("data").Values<string>().ToArray();
            }
            int index = 0;
            foreach (string hex in hexArray)
            {
                byte[] block = HexToBytes(hex);
                if (index >= BLOCKS_COUNT) throw new InvalidDataException($"Block count must be equal to {BYTES_IN_BLOCK}.");
                if (block.Length != BYTES_IN_BLOCK) throw new InvalidDataException($"Hex block must contain {BYTES_IN_BLOCK} bytes.");
                block.CopyTo(result, index * BYTES_IN_BLOCK);
                index++;
            }
            if (index < BLOCKS_COUNT) throw new InvalidDataException($"Block count must be equal to {BYTES_IN_BLOCK}.");

            return result;
        }

        public static byte[] HexToBytes(string hex)
        {
            var numberChars = hex.Length;
            var hexAsBytes = new byte[numberChars / 2];
            for (var i = 0; i < numberChars; i += 2)
                hexAsBytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            return hexAsBytes;
        }
    }
}
