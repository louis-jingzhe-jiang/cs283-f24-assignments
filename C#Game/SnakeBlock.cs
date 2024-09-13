using System;
using System.Drawing;
using System.Windows.Forms;

public class SnakeBlock
{
    private float[] _pos;
    public static int sideLength = 15;

    public float[] pos
    {
        get { return _pos; }
        set { _pos = value; }
    }

    public SnakeBlock()
    {

    }

    public SnakeBlock(float[] pos)
    {
        _pos = pos;
    }

    /**
     * Update the position of the block using delta time and velocity
     */
    public void Update(float dt, float[] vel)
    {
        float x = dt * vel[0] + _pos[0];
        float y = dt * vel[1] + _pos[1];
        _pos = new float[] {x, y};
    }

    /**
     * Update the position of the block using a position
     */
    public void Update(float[] pos)
    {
        _pos = pos;
    }

    public void Draw(Graphics g)
    {
        Color c = ColorTranslator.FromHtml("#ffee00");
        Brush brush = new SolidBrush(c);
        g.FillRectangle(brush, (int)_pos[0], (int)_pos[1], sideLength, sideLength);
    }

    public float[] Center()
    {
        return new float[]
        {
            _pos[0] + (float) sideLength / 2,
            _pos[1] + (float) sideLength / 2
        };
    }
}