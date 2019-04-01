namespace core.message
{
    public class PlayerStateMessage
    {
        public string PlayerId { get; set; }

        public float X { get; set; }

        public float Y { get; set; }
        
        public float Z { get; set; }
        
        public float RotationX { get; set; }

        public float RotationY { get; set; }

        public float RotationZ { get; set; }
    }
}