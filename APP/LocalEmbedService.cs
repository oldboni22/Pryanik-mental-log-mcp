using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.Tokenizers;

namespace APP;

public interface ILocalEmbedService
{
    float[] GenerateEmbedding(string text);
}

public sealed class LocalEmbedService : IDisposable, ILocalEmbedService
{
    private static readonly string Dir = Path.Combine(AppContext.BaseDirectory, "Models");
    
    private static readonly string ModelPath = Path.Combine(Dir, "model_quantized.onnx"); 
    
    private static readonly string TokenizerPath = Path.Combine(Dir, "vocab.txt");  
    
    private readonly InferenceSession _session;
    
    private readonly Tokenizer _tokenizer;

    public LocalEmbedService()
    {
        _session = new InferenceSession(ModelPath);
        _tokenizer = BertTokenizer.Create(TokenizerPath);
    }
    
    public float[] GenerateEmbedding(string text)
    {
        var ids = _tokenizer.EncodeToIds(text)
            .Select(id => (long)id)
            .ToArray();
        
        var length = ids.Length;
        
        var inputIdsTensor = new DenseTensor<long>(ids, new[] { 1, length });
        var attentionMaskTensor = new DenseTensor<long>(Enumerable.Repeat(1L, length).ToArray(), new[] { 1, length });
        
        var inputs = new[]
        {
            NamedOnnxValue.CreateFromTensor("input_ids", inputIdsTensor),
            NamedOnnxValue.CreateFromTensor("attention_mask", attentionMaskTensor)
        };
        
        using var results = _session.Run(inputs);
        
        var outputTensor = results.First().AsTensor<float>();
        
        return PoolAndNormalize(outputTensor, length);
    }
    
    private static float[] PoolAndNormalize(Tensor<float> tensor, int sequenceLength)
    {
        int dimensions = tensor.Dimensions[2]; // Будет 512 для выбранной модели
        var pooledVector = new float[dimensions];
        
        for (int i = 0; i < sequenceLength; i++)
        {
            for (int d = 0; d < dimensions; d++)
            {
                pooledVector[d] += tensor[0, i, d];
            }
        }
        
        float sumOfSquares = 0f;
        for (int d = 0; d < dimensions; d++)
        {
            pooledVector[d] /= sequenceLength;
            sumOfSquares += pooledVector[d] * pooledVector[d];
        }
        
        float magnitude = MathF.Sqrt(sumOfSquares);
        for (int d = 0; d < dimensions; d++)
        {
            pooledVector[d] /= MathF.Max(magnitude, 1e-12f);
        }

        return pooledVector;
    }

    public void Dispose()
    {
        _session.Dispose();
    }
}