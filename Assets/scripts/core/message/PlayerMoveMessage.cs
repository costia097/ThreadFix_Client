using System.Diagnostics.CodeAnalysis;

namespace core.message
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class PlayerMoveMessage
    {
        public string playerId { get; set; }

        public float x { get; set; }

        public float y { get; set; }
    }
}