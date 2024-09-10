using System;
using System.Drawing;
using System.Windows.Forms;

public class SnakeBlock 
{
    private int _direction;

    private float[] _pos;

    public int direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

    public SnakeBlock(int direction, int[] pos) 
    {
        _direction = direction;
        _pos = pos;
    }

    public void Reset()
    {

    }

    public void Update(float dt)
    {

    }

    public void Draw(Graphics g)
    {

    }
}