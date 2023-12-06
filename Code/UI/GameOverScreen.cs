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
                if (backToMenu.Bounds.Contains(mouseState.Position))
                {
                    this.shouldExitToMenu = true;
                }
                potentialSelection = false;

            }
        }
        else
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (backToMenu.Bounds.Contains(mouseState.Position))
                    this.potentialSelection = true;              
            }
        }

    }

    public void Draw()
    {
        DynamicSpriteFont font = ResourcesUi.FontSystem.GetFont(fontSize);
        Vector2 position = new Vector2((this.windowSize.X / 2) - (textSize.X / 2), this.windowSize.Y / 3);
        Rectangle textBgArea = new Rectangle((int)position.X - 8, (int)position.Y - 8, ((int)textSize.X) + 16, ((int)textSize.Y) + 16);
        GameWindow.spriteBatchUi.Draw(GameWindow.whitePixelTexture, textBgArea, ColorConfig.pallet0[0]);        
        GameWindow.spriteBatchUi.DrawString(font, text, position, ColorConfig.pallet0[2]);
        this.backToMenu.Draw();
    }

}