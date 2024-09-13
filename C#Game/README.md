# Assignment 2

## Snake game description

The game automatically starts when the program runs. The player uses wasd or up/down/left/right arrow keys to control the direction of the snake. The snake can always turn left or right (from its point of view), but cannot turn backwards. The snake will gain length when it eats the snake food shining on the screen. The snake is allowed to bump into itself but not allowed to bump into the boundary of the screen. The velocity of the snake will gradually increase as game time increase, making the game harder and harder. The counter for the snake's length is at the bottom of the screen.

## How to build / run

In the Visual Studio menu "Tools->Command Line->Developer PowerShell", open the developer PowerShell. Then run the following line to compile:

```csc *.cs```

After compiling, run 
```./Window.exe```

to run the program.

## Gameplay GIF

![](https://github.com/louis-jingzhe-jiang/cs283-f24-assignments/blob/main/C#Game/gameplay.gif)
