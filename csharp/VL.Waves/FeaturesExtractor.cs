using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWaves.FeatureExtractors;
using NWaves.FeatureExtractors.Base;
using NWaves.FeatureExtractors.Options;
using NWaves.Signals;


namespace VL.Waves
{
    public class FeaturesExtractor
    {


        public List<float[]> MFCCExtractor(DiscreteSignal signal,  int samplingRate=48000, int featureCount = 5, int frameSize= 256, int hopSize= 50, int filterBankSize= 8)
        {
            var mfccOptions = new MfccOptions
            {
                SamplingRate = samplingRate,
                FeatureCount = featureCount,
                FrameSize = frameSize,
                HopSize = hopSize,
                FilterBankSize = filterBankSize
            };

            var mfccExtractor = new MfccExtractor(mfccOptions);
            var mfccVectors = mfccExtractor.ComputeFrom(signal);
            var onlineMfccExtractor = new OnlineFeatureExtractor(new MfccExtractor(mfccOptions));
            List<float[]> onlineMfccVectors = new List<float[]>();

            var i = 0;
            while (i < signal.Length)
            {
                // emulating online blocks with different sizes:
                var size = (i + 1) * 15;
                var block = signal.Samples.Skip(i).Take(size).ToArray();

                var newVectors = onlineMfccExtractor.ComputeFrom(block);

                onlineMfccVectors.AddRange(newVectors);

                i += size;
            }
            var diff = mfccVectors.Zip(onlineMfccVectors, (e, o) => e.Zip(o, (f1, f2) => f1 - f2).Sum());

            return onlineMfccVectors;
        }
    }
}
