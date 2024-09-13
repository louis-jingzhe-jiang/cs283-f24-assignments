using System;
using System.Drawing;
using System.Windows.Forms;

public class Snake
{
    // snake length
    private int _length;

    // the position where the head is at
    private float[] _headPos;
    
    // velocity (direction of snake determined by velocity)
    private float[] _vel;

    // the list of snake body blocks (including the head)
    private SnakeBlock[] _snakeBlocks;

    public int length
    {
        get { return _length; }
    }

    /**
     * Initialize the snake
     */
    public void Reset()
    {
        _length = 1;
        _headPos = new float[] {320f, 240f};
        _vel = new float[] {50f, 0f};
        _snakeBlocks = new SnakeBlock[100];
        _snakeBlocks[0] = new SnakeBlock(_headPos);
    }

    public void Update(float dt)
    {
        for (int i = _length - 1; i > 0; i--)
        {
            /* 
             * Update snake body by moving each body block to the location 
             * of the body block ahead of it
             */
            _snakeBlocks[i].Update(_snakeBlocks[i - 1].pos);
        }
        _snakeBlocks[0].Update(dt, _vel); // update snake head at last
        // speed up the snake
        _vel[0] *= (1 + dt / 1000);
        _vel[1] *= (1 + dt / 1000);
    }

    public void Increase()
    {
        _snakeBlocks[_length] = new SnakeBlock(
            new float[] { _vel[0] * -50, _vel[1] * -50 }
            );
        _length += 1;
    }

    public void Up()
    {
        if (_vel[1] == 0)
        {
            if (_vel[0] > 0)
            {
                _vel[1] = _vel[0] * (-1);
            }
            else
            {
                _vel[1] = _vel[0];
            }
            _vel[0] = 0;
        }
    }

    public void Down()
    {
        if (_vel[1] == 0)
        {
            if (_vel[0] > 0)
            {
                _vel[1] = _vel[0];
            }
            else
            {
                _vel[1] = _vel[0] * (-1);
            }
            _vel[0] = 0;
        }
    }

    public void Left()
    {
        if (_vel[0] == 0)
        {
            if (_vel[1] > 0)
            {
                _vel[0] = _vel[1] * (-1);
            }
            else
            {
                _vel[0] = _vel[1];
                
            }
            _vel[1] = 0;
        }
    }

    public void Right()
    {
        if (_vel[0] == 0)
        {
            if (_vel[1] > 0)
            {
                _vel[0] = _vel[1];
            }
            else
            {
                _vel[0] = _vel[1] * (-1);
            }
            _vel[1] = 0;
        }
    }

    public void Draw(Graphics g)
    {
        for (int i = 0; i < _length; i++)
        {
            _snakeBlocks[i].Draw(g);
        }
    }

}