namespace Domain.Interfaces;

public interface IText
{
    public string Text { get; init; }
    
    public int TextLength { get; set; }
}
