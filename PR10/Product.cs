using Avalonia.Media;

namespace PR10;

public class Product
{
    public int ProductId { get; set; }
    public string Article { get; set; }
    public string ProductName { get; set; }
    public string Unit { get; set; }
    public decimal Price { get; set; }
    public int MaxDiscount { get; set; }
    public string Producer { get; set; }
    public string Supplier { get; set; }
    public string Category { get; set; }
    public int CurrentDiscount { get; set; }
    public int Amount { get; set; }
    public string Description { get; set; }
    public byte[] ImagePreview { get; set; }
}