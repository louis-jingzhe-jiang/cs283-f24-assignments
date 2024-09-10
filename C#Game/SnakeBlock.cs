using System;
using System.Drawing;
using System.Windows.Forms;

public class SnakeBlock {
    private float[] _pos;

    public float[] pos {
        get { return _pos; }
        set { _pos = value; }
    }

    public SnakeBlock(int direction, int[] pos) {
        _direction = direction;
        _pos = pos;
    }

    public void Reset() {

    }

    /**
     * Update the position of the block using delta time and velocity
     */
    public void Update(float dt, float[] vel) {
        float x = dt * vel[0] + _pos[0];
        float y = dt * vel[1] + _pos[1];
        _pos = new float[] {x, y};
    }

    /**
     * Update the position of the block using a position
     */
    public void Update(float[] pos) {
        _pos = pos;
    }

    public void Draw(Graphics g) {

    }
}