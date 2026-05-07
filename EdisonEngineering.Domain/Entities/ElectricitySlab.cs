namespace EdisonEngineering.Domain.Entities;

public class ElectricitySlab
{
    public int Id { get; set; }

    public int MinUnit { get; set; }
    public int MaxUnit { get; set; }

    public decimal Rate { get; set; }
}