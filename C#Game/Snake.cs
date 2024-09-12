using System;
using System.Drawing;
using System.Windows.Forms;

public class Snake {

    // snake length
    private int _length;

    // the position where the head is at
    private float[] _headPos;
    
    // velocity (direction of snake determined by velocity)
    private float[] _vel;

    // the list of snake body blocks (including the head)
    private ArrayList<SnakeBlock> _snakeBlocks;

    /**
     * Initialize the snake
     */
    public void Reset() {
        _direction = 0;
        _length = 1;
        _headPos = {0.0, 0.0};
        _vel = {1.0, 0.0};
        _snakeBlocks = new ArrayList<SnakeBlock>();
        _snakeBlocks.Add(new SnakeBlock(_direction, _headPos));
    }

    public void Update(float dt) {
        for (int i = _length - 1; i > 0; i++) {
            /* 
             * Update snake body by moving each body block to the location 
             * of the body block ahead of it
             */
            _snakeBlocks[i].Update(_snakeBlocks[i - 1].pos);
        }
        _snakeBlocks[0].Update(dt, _vel); // update snake head at last
    }

    public void Draw(Graphics g) {
        foreach (SnakeBlock sb in _snakeBlocks) {
            sb.Draw(g);
        }
    }

}