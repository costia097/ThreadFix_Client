using System.Collections.Generic;

namespace core.message
{
    public class BombExplosionMessage
    {
        public string BombId { get; set; }
        
        public List<EnemyHittedMessage> EnemiesHitted { get; set; }
    }
}