
using VL.Audio;
using NWaves;
using NWaves.Signals;
using System.Collections.Generic;

namespace VL.Waves.Audio
{
    public class Utils
    {
        
        
        public static DiscreteSignal ToDiscrete(int samplingRate, AudioSignal Signal)
        {
           
            IReadOnlyList<float> buffer = new List<float>();
            int lastIntCount = 0;
            Signal.GetInternalReadBuffer(out buffer, out lastIntCount);
            DiscreteSignal DSignal = new DiscreteSignal(samplingRate, buffer);
           
            return DSignal;
        }

        public static AudioSignal FromDiscrete(DiscreteSignal discrete)
        { 
            AudioSignalGeneratorRegion<>
            AudioSignal Signal = null;
            float[] samples = discrete.Samples;
            List<SigParamBase> sigParam = AudioSignal.GetInputParams(Signal).ToList();
            sigParam.
            
        }
    }
}