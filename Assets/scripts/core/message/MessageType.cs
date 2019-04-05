namespace core.message
{
    public enum MessageType
    {
        PlayerState, 
        PlayerJoin, 
        FirstPlayersSync, 
        FirstEnemiesSync,
        EnemyState, 
        PlayerWave, 
        EnemyHitted, 
        FirstMapSync,
        MapChanged,
        PlayerThrowBomb,
        BombExploded
    }
}