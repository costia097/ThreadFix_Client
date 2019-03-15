using System.Diagnostics.CodeAnalysis;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class MovementDto
{
    public long playerId { get; set; }

    public float x { get; set; }

    public float y { get; set; }
}