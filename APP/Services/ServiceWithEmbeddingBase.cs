namespace APP.Services;

public abstract class ServiceWithEmbeddingBase(IEmbedService embedService)
{
    protected IEmbedService EmbedService => embedService;
    
    protected float CalculateSimilarity(float[] v1, float[] v2)
    {
        return v1.Zip(v2, (a, b) => a * b).Sum();
    }
}