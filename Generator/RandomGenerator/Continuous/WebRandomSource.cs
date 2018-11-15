using Generator.ResourceAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Generator.RandomGenerator.Continuous
{
    public class WebRandomSource : IUniformDistributionGenerator
    {
        private IWebResourceAccessDetails mResourceAccessDetails;

        public WebRandomSource(IWebResourceAccessDetails resourceAccessDetails)
        {
            mResourceAccessDetails = resourceAccessDetails;
        }

        private IEnumerable<byte> mBytesSource;
        private IEnumerable<byte> BytesSource
        {
            get
            {
                if (mBytesSource == null)
                    mBytesSource = GetBytesSource();
                return mBytesSource;
            }
        }
        internal IEnumerable<byte> GetBytesSource()
        {
            while (true)
            {
                var request = WebRequest.Create(mResourceAccessDetails.RequestUrl);
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK) throw new WebException($"Web request failed with startus {response.StatusCode}: {response.StatusDescription}");

                    byte[] numbers;
                    using (var stream = response.GetResponseStream())
                        numbers = mResourceAccessDetails.ProcessResponseToNumbers(stream);

                    foreach (var item in numbers)
                        yield return item;
                }
            }
        }

        public double Next()
        {
            byte[] b = BytesSource.Take(4).ToArray();
            return (double)BitConverter.ToUInt32(b, 0) / UInt32.MaxValue;
        }
    }
}
