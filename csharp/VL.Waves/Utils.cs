///For examples, see:
///https://thegraybook.vvvv.org/reference/extending/writing-nodes.html#examples
//using CommunityToolkit.HighPerformance;

using NWaves.Signals;
using VL.Lib.Collections;


namespace VL.Waves;

public static class Utils
{
    
    public static DiscreteSignal GetRange(DiscreteSignal signal, int index, int count)
        
    {

        DiscreteSignal newSignal = null;
        if(signal != null)
        {
            if (index < 0)
            {
                index = 0;
                throw new ArgumentOutOfRangeException("index <0 , index was set to 0");
            }
            if (count > signal.Length - index)
            {
                throw new ArgumentOutOfRangeException("count can't be greater than signal length - index");
            }
            var signalSegment = signal.Samples.Skip(index).Take(count);

            newSignal = new DiscreteSignal(signal.SamplingRate, signalSegment);
        }
        
        return newSignal;
    }
    public static class AudioUtils
    {
        public static float HzToMel(float hz)
        {
            return 2595 * (float)Math.Log10(1 + hz / 700);
        }

        // Method to convert Mel to Hz
        public static float MelToHz(float mel)
        {
            return 700 * (float)(Math.Pow(10, mel / 2595) - 1);
        }
        public static Spread<double[]> CreateMelFilterBank(int sr, int nFft, int nMels, float fMin, float fMax)
        {
            // Calculate Mel frequencies
            Spread<float> melPoints = Enumerable.Range(0, nMels + 2)
                                            .Select(i => HzToMel(fMin + i * (fMax - fMin) / (nMels + 1)))
                                            .ToSpread();

            Spread<float> hzPoints = melPoints.Select(i => MelToHz(i)).ToSpread();

            // Convert Hz to FFT bin numbers
            Spread<int> binPoints = hzPoints.Select(hz => (int)Math.Floor((nFft + 1) * hz / sr)).ToSpread();

            // Initialize the filter bank
            List<double[]> filters = new List<double[]>(nMels);
            for (int m = 1; m <= nMels; m++)
            {
                double[] filter = new double[nFft];

                int fMMinus = binPoints[m - 1];
                int fM = binPoints[m];
                int fMPlus = binPoints[m + 1];

                for (int k = fMMinus; k < fM; k++)
                {
                    filter[k] = (double)(k - fMMinus) / (fM - fMMinus);
                }
                for (int k = fM; k < fMPlus; k++)
                {
                    filter[k] = (double)(fMPlus - k) / (fMPlus - fM);
                }

                filters.Add(filter);
            }

            return filters.ToSpread();
        }

        public static Spread<float> ApplyMelFilterBank(Spread<float> magnitudes, Spread<double[]> melFilters)
        {
            int fftSize = magnitudes.Count;
            int melFilterCount = melFilters.Count;

            float[] melSpectrum = new float[melFilterCount];

            for (int i = 0; i < melFilterCount; i++)
            {
                double[] filter = melFilters[i];
                double filterSum = 0.0;

                for (int j = 0; j < filter.Length; j++)
                {
                    if (j < fftSize) // Ensure we are within bounds
                    {
                        filterSum += (float)magnitudes[j] * (float)filter[j];
                    }
                }

                melSpectrum[i] = (float)filterSum;
            }

            return melSpectrum.ToSpread();
        }

    }
}