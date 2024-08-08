using System;

namespace GameLogic.Battle
{
    public class RandomUtils
    {
        public static Random Random = new Random();
        // public static bool Result01(float probability)
        // {
        //     return probability > 0 && (probability >= 1 || random.Next() < probability);
        // }

        public static bool Result(float probability)
        {
            return probability > 0 && (probability >= 100 || Random.Next(0, 100) < probability);
        }
    }
}