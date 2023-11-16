using System;
using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class MainMenu_Button
{

    //private const double StartYPercentage = 0.25;
    private const double BufferPercantage = 0.025;
    private const double WidthPercentage = 0.455;
    private const double HeightPercentage = WidthPercentage / 2;
    private const double InnerBufferPercentage = HeightPercentage / 8;

    private static Texture2D frameTexture;

    public bool highlight = false;
    public readonly bool valid = true;

    public bool IsPicked {get; private set;} = false;
    public Rectangle Bounds {get {return this.drawArea;}}
    public readonly int index;

    Rectangle drawArea;
    Rectangle textureDrawArea;
    private const int numberOfOptions = 2;

    private Texture2D optionTexture;
    private String text;

    

    public MainMenu_Button(Point windowSize, int index, String text)
    {
        this.index = index;
        frameTexture ??= Texture2D.FromFile(GameWindow.graphicsDevice, "Data/Texture/UI/frame2.png");
        this.AdjustDrawArea(windowSize);
        this.text = text;
    }

    public void AdjustDrawArea(Point windowSize)
    {
        int bufferPxSize = (int)(windowSize.X * BufferPercantage);
        int widthPxSize = (int)(windowSize.X * WidthPercentage);
        int heightPxSize = (int)(windowSize.Y * HeightPercentage);
        int innerWidthBufferPxSize = (int)(windowSize.X * InnerBufferPercentage);
        int innerHeightBufferPxSize = (int)(windowSize.Y * InnerBufferPercentage);

        int combinedHeight = bufferPxSize * (numberOfOptions - 1) + heightPxSize * numberOfOptions;
        int startX = (windowSize.X - widthPxSize) / 2;
        int startY = (windowSize.Y - combinedHeight) / 2;

        this.drawArea = new Rectangle
        (
            startX,
            startY + bufferPxSize * (index - 1) + heightPxSize * index,
            widthPxSize,
            heightPxSize
        );

        this.textureDrawArea = new Rectangle
        (
            drawArea.X + innerWidthBufferPxSize,
            drawArea.Y + innerHeightBufferPxSize,
            drawArea.Width - innerWidthBufferPxSize * 2,
            drawArea.Height - innerHeightBufferPxSize * 2
        );

    }

    public void Draw()
    {
        GameWindow.spriteBatch.Draw(GameWindow.whitePixelTexture, this.drawArea, Color.Gray);
        GameWindow.spriteBatch.Draw(GameWindow.whitePixelTexture, this.textureDrawArea, Color.Red);

        int fontSize = 56;
        SpriteFontBase font = ResourcesUi.FontSystem.GetFont(fontSize);
        Vector2 centerOfButtonVec = this.textureDrawArea.Center.ToVector2();
        Vector2 fontVec = new(font.MeasureString(text).Length() / 2, fontSize / 2);
        Vector2 vec = centerOfButtonVec - fontVec;
        GameWindow.spriteBatch.DrawString(font, this.text, vec, Color.Black);   
    }

}