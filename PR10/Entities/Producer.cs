namespace PR10;

public class Producer
{
    public int ProducerId { get; set; }
    public string ProducerName { get; set; }

    public override string ToString()
    {
        return ProducerName;
    }
}