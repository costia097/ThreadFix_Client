using System;

//[Serializable]
public class MovementDto
{
    private long playerId;
    private float x;
    private float y;

    public long PlayerId
    {
        get { return playerId; }
        set { playerId = value; }
    }

    public float X
    {
        get { return x; }
        set { x = value; }
    }

    public float Y
    {
        get { return y; }
        set { y = value; }
    }
}