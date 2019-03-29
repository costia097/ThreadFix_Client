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
        
        public float z { get; set; }
        
        public float rotationX { get; set; }

        public float rotationY { get; set; }

        public float rotationZ { get; set; }
    }
}