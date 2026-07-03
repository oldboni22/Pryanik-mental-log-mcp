using System.Buffers;
using System.Numerics.Tensors;
using System.Runtime.InteropServices;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.Tokenizers;

namespace APP;

public interface IEmbedService
{
    byte[] GenerateEmbedding(string text);
    
    int ByteBufferSize { get; }
}

public sealed class LocalEmbedService : IDisposable, IEmbedService
{
    private static readonly string Dir = Path.Combine(AppContext.BaseDirectory, "Models");
    
    private static readonly string ModelPath = Path.Combine(Dir, "model_quantized.onnx"); 
    
    private static readonly string TokenizerPath = Path.Combine(Dir, "vocab.txt");
    
    private readonly InferenceSession _session = new(ModelPath);
    
    private readonly Tokenizer _tokenizer = BertTokenizer.Create(TokenizerPath);

    public int ByteBufferSize => 768 * sizeof(float);
    
    public byte[] GenerateEmbedding(string text)
    {
        var encodedIds = _tokenizer.EncodeToIds(text);
        var length = encodedIds.Count;
        
        if (length == 0) return [];
        
        var idsBuffer = ArrayPool<long>.Shared.Rent(length);
        var maskBuffer = ArrayPool<long>.Shared.Rent(length);

        var byteBuffer = new byte[ByteBufferSize];
        
        var floatSpan = MemoryMarshal.Cast<byte, float>(byteBuffer.AsSpan());
        
        try
        {
            for (int i = 0; i < length; i++)
            {
                idsBuffer[i] = encodedIds[i];
                maskBuffer[i] = 1L;
            }
            
            var inputIdsTensor = new DenseTensor<long>(idsBuffer.AsMemory(0, length), new[] { 1, length });
            var attentionMaskTensor = new DenseTensor<long>(maskBuffer.AsMemory(0, length), new[] { 1, length });
            
            var inputs = new[]
            {
                NamedOnnxValue.CreateFromTensor("input_ids", inputIdsTensor),
                NamedOnnxValue.CreateFromTensor("attention_mask", attentionMaskTensor)
            };
            
            using var results = _session.Run(inputs);
            var outputTensor = (DenseTensor<float>)results[0].AsTensor<float>();
            
            var tensorSpan = outputTensor.Buffer.Span;
            var dimensions = outputTensor.Dimensions[2];
            
            if (floatSpan.Length < dimensions)
            {
                throw new ArgumentException($"Destination span is too short. Expected at least {dimensions} elements.");
            }
        
            PoolAndNormalize(tensorSpan, length, dimensions, floatSpan);
        }
        finally
        {
            ArrayPool<long>.Shared.Return(idsBuffer);
            ArrayPool<long>.Shared.Return(maskBuffer);
        }

        return byteBuffer;
    }
    
    private static void PoolAndNormalize(
        ReadOnlySpan<float> tensorSpan, int sequenceLength, int dimensions, Span<float> floatSpan)
    {
        floatSpan.Clear();
        
        //Calculate a sum of all token vectors
        for (int i = 0; i < sequenceLength; i++)
        {
            //Take current token's vector from plain tensor [xxxx |...| xxxx]
            var tokenVector = tensorSpan.Slice(i * dimensions, dimensions);
            TensorPrimitives.Add(floatSpan, tokenVector, floatSpan);
        }
        
        TensorPrimitives.Divide(floatSpan, sequenceLength, floatSpan);
        
        var norm = TensorPrimitives.Norm(floatSpan);
        var scale = MathF.Max(norm, 1e-12f);
        
        TensorPrimitives.Divide(floatSpan, scale, floatSpan);
    }

    public void Dispose()
    {
        _session.Dispose();
    }
}