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
}
