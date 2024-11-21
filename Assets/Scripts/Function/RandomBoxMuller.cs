static public class Bernoulli
{

    public static bool Probability(this System.Random me, float probability)
    {
        float rand = (float)(me.Next(100)) * 0.01f;
        return rand >= probability;
    }
}
