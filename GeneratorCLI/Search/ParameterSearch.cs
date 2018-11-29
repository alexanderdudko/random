using System;
using System.Collections.Generic;
using System.Text;

namespace GeneratorCLI.Search
{
    class ParameterSearch
    {
        public static double SearchParameterValue(Func<double, double> f, double targetY, double minX, double maxX, double precisionY, int maxStepsCount = 1000)
        {
            double fmaxX = f(maxX);
            double fminX = f(minX);
            bool increasing = fmaxX > fminX;

            // Check if target is outside of bounds
            if (increasing && targetY <= fminX || !increasing && targetY >= fminX)
                return minX;
            else if (increasing && targetY >= fmaxX || !increasing && targetY <= fmaxX)
                return maxX;

            int stepsCount = 0;
            double currentX = minX;
            while (stepsCount < maxStepsCount)
            {
                // Take middle point as a guess
                currentX = (maxX + minX) / 2;

                // Evaluate function value
                double currentY = f(currentX);

                // Check if target found
                if (Math.Abs(currentY - targetY) <= precisionY) return currentX;

                // Reduce search area
                if (increasing && targetY >= currentY || !increasing && targetY < currentY)
                    minX = currentX;
                else
                    maxX = currentX;

                stepsCount++;
            }
            return currentX;
        }
    }
}
