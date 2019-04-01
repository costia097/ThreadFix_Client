using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace core.message
{
    public class FirstPlayersSyncMessage
    {
        public List<ServerPlayer> Players
        {
            get;
            set;
        }
        
    }
}