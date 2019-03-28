using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace core.message
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class FirstSyncMessage
    {
        public List<ServerPlayer> players { get; set; }
    }
}