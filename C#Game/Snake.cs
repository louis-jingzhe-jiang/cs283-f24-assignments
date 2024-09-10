using System;
using System.Drawing;
using System.Windows.Forms;

public class Snake
{
    private int _direction;

    private int _length;

    private float[] _headPos;

    private ArrayList<SnakeBlock> _snakeBlocks;

    public void Reset()
    {
        _direction = 0;
        _length = 1;
        _headPos = {0.0, 0.0};
        _snakeBlocks = new ArrayList<SnakeBlock>();
        _snakeBlocks.Add(new SnakeBlock(_direction, _headPos));
    }

    public void Update(float dt)
    {
        for (int i = 0; i < _length; i++)
        {
            if (i - 1 >= 0 && _snakeBlocks[i - 1].)
        }
    }

    public void Draw(Graphics g)
    {
        foreach (SnakeBlock sb in _snakeBlocks) 
        {
            sb.Draw(g);
        }
    }

}