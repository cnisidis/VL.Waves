
using VL.Audio;
using NWaves;
using NWaves.Signals;
using System.Collections.Generic;
using NWaves.Audio;

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

        public static void WriteWav(DiscreteSignal signal, string filepath)
        {
            WaveFile waveFileOut;
            //This is how I read it:
            WaveFile shiftedWav;
            using (var stream = new FileStream(filepath, FileMode.Open))
            {
                shiftedWav = new WaveFile(stream);
            }
            DiscreteSignal shiftedSignal = shiftedWav[Channels.Left];

            DiscreteSignal mergedSignal = shiftedSignal + outSignal;

            waveFileOut = new WaveFile(mergedSignal);
            using (var stream = new FileStream("merged.wav", FileMode.Create))
            {
                waveFileOut.SaveTo(stream);
            }
        }

        /*
        public static Stream ToStream(DiscreteSignal Signal)
        {
            Signal.Samples
        }
        
        public static AudioSignal FromDiscrete(DiscreteSignal discrete)
        { 
            AudioSignalGeneratorRegion<>
            AudioSignal Signal = null;
            float[] samples = discrete.Samples;
            List<SigParamBase> sigParam = AudioSignal.GetInputParams(Signal).ToList();
            sigParam.
            
        }
        */
    }
}