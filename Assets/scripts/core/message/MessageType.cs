using System.Diagnostics.CodeAnalysis;

namespace core.message
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum MessageType
    {
        PLAYER_MOVE, PLAYER_JOIN,FIRST_SYNC
    }
}