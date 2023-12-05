using System;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

class GameOverScreen
{

    private const int fontSize = 42;

    public bool shouldExitToMenu {get; private set;} = false;

    readonly string text;
    Point windowSize;
    Vector2 textSize;
    MainMenu_Button backToMenu;

    

    public GameOverScreen(int night, Point windowSize)
    {
        this.text = $"-- Game Over --\n     Night : {night}\n";
        this.windowSize = windowSize;
        this.textSize = ResourcesUi.FontSystem.GetFont(fontSize).MeasureString(this.text);
        backToMenu = new MainMenu_Button(windowSize, 1, "Exit");
    }

    public void AdjustDrawArea(Point windowSize)
    {
        this.windowSize = windowSize;
        this.backToMenu.AdjustDrawArea(windowSize);
    }


    private bool potentialSelection = false;
    public void Update(MouseState mouseState)
    {

        if (potentialSelection)
        {
            if (mouseState.LeftButton == ButtonState.Released)
            {
                Console.WriteLine($"1   {this.GetHashCode()}");
                if (backToMenu.Bounds.Contains(mouseState.Position))
                {
                    Console.WriteLine("     contains");
                    this.shouldExitToMenu = true;
                }

                potentialSelection = false;

            }
        }
        else
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Console.WriteLine($"2   {this.GetHashCode()}");
                if (backToMenu.Bounds.Contains(mouseState.Position))
                {
                    Console.WriteLine($"     contains {potentialSelection}");
                    this.potentialSelection = true;
                    Console.WriteLine($"     contains {potentialSelection}");
                }                
            }
        }

    }

    public void Draw()
    {
        DynamicSpriteFont font = ResourcesUi.FontSystem.GetFont(fontSize);
        Vector2 position = new Vector2((this.windowSize.X / 2) - (textSize.X / 2), this.windowSize.Y / 3);
        GameWindow.spriteBatchUi.DrawString(font, text, position, Color.Black);
        this.backToMenu.Draw();
    }

}