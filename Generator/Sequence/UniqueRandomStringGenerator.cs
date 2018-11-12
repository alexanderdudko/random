using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.Sequence
{
    public class UniqueRandomStringGenerator
    {
        private int mLength;
        private RandomStringGenerator mGenerator = new RandomStringGenerator();
        private HashSet<string> mHistory = new HashSet<string>();

        public UniqueRandomStringGenerator(int length)
        {
            mLength = length;
        }

        public string GetUniqueString(int maxTriesCount = 1000)
        {
            string next;
            int triesCount = 0;
            do
            {
                next = mGenerator.GetString(mLength);
                triesCount++;
            } while (mHistory.Contains(next) || triesCount > maxTriesCount);
            mHistory.Add(next);

            if (triesCount > maxTriesCount)
                throw new Exception("Reached max tries count while generating unique string");

            return next;
        }

        public void ClearHistory()
        {
            mHistory.Clear();
        }
    }
}
