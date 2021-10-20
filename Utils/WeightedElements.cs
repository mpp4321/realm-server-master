using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RotMG.Utils
{
    class WeightedElements<T>
    {

        private List<(float, T)> weights = new List<(float, T)>();

        public WeightedElements(IEnumerable<(float, T)> items)
        {
            float totalWeight = items.Sum(a => a.Item1);
            foreach(var itemData in items)
            {
                weights.Add(( itemData.Item1 / totalWeight, itemData.Item2 ));
            }
        }

        public T Next()
        {
            float target = MathUtils.NextFloat();
            float lower = 0;
            float higher = 0;

            foreach(var weightData in weights)
            {
                higher += weightData.Item1;
                if(target >= lower && target <= higher)
                {
                    return weightData.Item2;
                }
                lower += weightData.Item1;
            }

            throw new Exception("Unable to find weight for key");
        }

    }
}
