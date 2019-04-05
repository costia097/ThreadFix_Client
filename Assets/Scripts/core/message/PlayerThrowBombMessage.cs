namespace core.message
{
    public class PlayerThrowBombMessage
    {
        public string PlayerName { get; set; }
        
        public string BombName { get; set; }
        
        public Direction Direction { get; set; }
    }
}