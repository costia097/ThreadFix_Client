using System.Diagnostics.CodeAnalysis;

namespace core.message
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PlayerJoinMessage
    {
        public string playerId { get; set; }
    }
}