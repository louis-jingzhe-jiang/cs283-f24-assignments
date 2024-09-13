using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

public class Game
{
    private Snake _snake;

    private Food _food;

    private bool _gameOver;

    private Image _mImg;

    public Game()
    {
        Setup();
        _mImg = Image.FromFile("AF_Grass.png"); // load once during initialization
    }

    public void Setup()
    {
        // create snake
        _snake = new Snake();
        // create snake food
        _food = new Food(640, 480);
        _gameOver = false;
    }

    public void Update(float dt)
    {
        // the coordinate of the center of the snake food
        float[] fCen = _food.Center();
        // the coordinate of the center of the snake head
        float[] sCen = _snake.HeadCenter();

        // if snake head overlap with food, increase length, reset snake food
        if (-(Food.diameter + SnakeBlock.sideLength) / 2 < fCen[0] - sCen[0] &&
            fCen[0] - sCen[0] < (Food.diameter + SnakeBlock.sideLength) / 2 &&
            -(Food.diameter + SnakeBlock.sideLength) / 2 < fCen[1] - sCen[1] &&
            fCen[1] - sCen[1] < (Food.diameter + SnakeBlock.sideLength) / 2)
        {
            _snake.Increase();
            _food.Reset(620, 430);
        }

        // if snake head hit window boundary, game over
        if (sCen[0] < 0 || sCen[0] > 640 ||
            sCen[1] < 0 || sCen[1] > 450)
        {
            _gameOver = true;
        }

        if (! _gameOver)
        {
            _food.Update(dt);
            _snake.Update(dt);
            //System.Console.WriteLine(_snake.vel[0] + ", " + _snake.vel[1]);
        }
    }

    public void Draw(Graphics g)
    {
        // draw background
        g.DrawImage(_mImg, 0, 0, 640, 480);
        // draw bottom infomation section
        Color bar = Color.Black;
        Brush brush = new SolidBrush(bar);
        g.FillRectangle(brush, 0, 450, Window.width, 30);

        Font font1 = new Font("Arial", 10);
        SolidBrush font1Brush = new SolidBrush(Color.White);
        StringFormat format1 = new StringFormat();
        format1.LineAlignment = StringAlignment.Center;
        format1.Alignment = StringAlignment.Center;
        g.DrawString("Snake Game\n Your Length: " + _snake.length , 
               font1, font1Brush,
               (float)(Window.width * 0.5),
               (float)(465),
               format1);

        if (! _gameOver) // game is not over
        {
            _food.Draw(g);
            _snake.Draw(g);
        }
        else // game is over
        {
            Font font2 = new Font("Arial", 46);
            SolidBrush font2Brush = new SolidBrush(Color.Black);

            StringFormat format2 = new StringFormat();
            format2.LineAlignment = StringAlignment.Center;
            format2.Alignment = StringAlignment.Center;

            g.DrawString("GAME OVER!\nThanks for playing!\nPress R to restart",
                font2, font2Brush,
                (float)(Window.width * 0.5),
                (float)(Window.height * 0.4),
                format2);
        }
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
        else if (key.KeyCode == Keys.R)
        {
            Setup();
        }
    }
}
