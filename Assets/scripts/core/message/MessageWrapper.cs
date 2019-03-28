using System.Diagnostics.CodeAnalysis;

namespace core.message
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class MessageWrapper
    {
        public long messageId { get; set; }
        
       public MessageType messageType{get; set; }
       
       public string payload { get; set; }
    }
}
 
       
                      