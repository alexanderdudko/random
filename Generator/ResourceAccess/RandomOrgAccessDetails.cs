using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Generator.ResourceAccess
{
    public class RandomOrgAccessDetails : IWebResourceAccessDetails
    {
        private const int NUMBERS_COUNT = 10000;

        public string RequestUrl => "https://www.random.org/integers/?num=10000&min=0&max=255&col=1&base=10&format=plain&rnd=new";

        public byte[] ProcessResponseToNumbers(Stream response)
        {
            byte[] result = new byte[NUMBERS_COUNT];
            int index = 0;
            using (var sr = new StreamReader(response))
            {
                while (!sr.EndOfStream && index < NUMBERS_COUNT)
                {
                    result[index] = byte.Parse(sr.ReadLine());
                    index++;
                }
            }
            return result;
        }
    }
}
