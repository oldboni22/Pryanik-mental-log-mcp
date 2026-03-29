namespace APP.Services;

public abstract class ServiceBase
{
    protected float CalculateSimilarity(float[] v1, float[] v2)
    {
        return v1.Zip(v2, (a, b) => a * b).Sum();
    }
}