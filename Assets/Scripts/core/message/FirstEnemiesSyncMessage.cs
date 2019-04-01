using System.Collections.Generic;
using core.entities;

namespace core.message
{
    public class FirstEnemiesSyncMessage
    {
        public List<Enemy> Enemies
        {
            get;
            set;
        }
    }
}