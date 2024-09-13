using System;
using System.Drawing;
using System.Windows.Forms;

public class Game
{
    private Snake _snake;

    private Food _food;

    public void Setup()
    {
        // create snake
        _snake = new Snake();
        _snake.Reset();
        _snake.Increase();
        _snake.Increase();
        _snake.Increase();
        _snake.Increase();
        _snake.Increase();
        // create snake food
        _food = new Food(640, 480);
    }

    public void Update(float dt)
    {
        _food.Update(dt);
        _snake.Update(dt);
        // if snake head overlap with food, increase length, reset snake food
        // if snake head overlap with snake body, game over
        // if snake head hit window boundary, game over
    }

    public void Draw(Graphics g)
    {
        _food.Draw(g);
        _snake.Draw(g);
    }

    public void MouseClick(MouseEventArgs mouse)
    {
        if (mouse.Button == MouseButtons.Left)
        {
            System.Console.WriteLine(mouse.Location.X + ", " + mouse.Location.Y);
        }
    }

    public void KeyDown(KeyEventArgs key)
    {
        if (key.KeyCode == Keys.D || key.KeyCode == Keys.Right)
        {
            _snake.Right();
        }
        else if (key.KeyCode== Keys.S || key.KeyCode == Keys.Down)
        {
            _snake.Down();
        }
        else if (key.KeyCode == Keys.A || key.KeyCode == Keys.Left)
        {
            _snake.Left();
        }
        else if (key.KeyCode == Keys.W || key.KeyCode == Keys.Up)
        {
            _snake.Up();
        }
    }
}
