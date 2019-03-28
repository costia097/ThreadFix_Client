using System.Diagnostics.CodeAnalysis;

namespace core
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ServerPlayer
    {
        public string name { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
    }
}