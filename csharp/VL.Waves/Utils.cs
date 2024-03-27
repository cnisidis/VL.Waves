///For examples, see:
///https://thegraybook.vvvv.org/reference/extending/writing-nodes.html#examples
//using CommunityToolkit.HighPerformance;
using NWaves;
using NWaves.Signals;
using NWaves.Signals.Builders.Base;
using System.Linq;

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
    /*
    public static List<float> OccurenciesAverage(IEnumerable<KeyValuePair<int, float>> values)
    {
        int prev_pair_key = 0;
        
        for (int i = 0; i<values.Count(); i++)
        {
            List<float> newValues = new List<float>();
            
            newValues.Add();
        }
    }
    */
}