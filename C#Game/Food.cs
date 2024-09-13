using System;
using System.Drawing;
using System.Windows.Forms;

public class Food
{
    private float[] _pos;
    // radius of the snake food
    public static int diameter = 20;

    private int[] _color;

    public float[] pos
    {
        get { return _pos; }
        set { _pos = value; }
    }
    
    /**
     * Initialize and set the position of the snake food
     */
    public Food(int winWidth, int winHeight)
    {
        _pos = new float[2];
        Reset(winWidth, winHeight);
        _color = new int[4];
        Random _rnd = new Random();
        for (int i = 1; i < 4; i++)
        {
            _color[i] = _rnd.Next(0, 255);
        }
        _color[0] = 191;
    }

    public void Reset(int winWidth, int winHeight)
    {
        Random _rnd = new Random();
        _pos[0] = _rnd.Next(20, winWidth);
        _pos[1] = _rnd.Next(20, winHeight - 30);
    }

    public void Update(float dt)
    {
        _color[1] += (int) (100 * dt);
        _color[2] += (int) (200 * dt);
        _color[3] += (int) (100 * dt);
        for (int i = 1; i < 4; i++)
        {
            if (_color[i] > 255)
            {
                _color[i] -= 255;
            }
        }
    }

    public void Draw(Graphics g)
    {
        Color c = Color.FromArgb(_color[0], _color[1], _color[2], _color[3]);
        Brush brush = new SolidBrush(c);
        g.FillEllipse(brush, (int)_pos[0], (int)_pos[1], diameter, diameter);
    }

    public float[] Center()
    {
        return new float[]
        {
            _pos[0] + (float) diameter / 2,
            _pos[1] + (float) diameter / 2
        };
    }
}