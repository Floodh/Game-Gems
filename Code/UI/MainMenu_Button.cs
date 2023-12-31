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
    private const double InnerBufferPercentage = HeightPercentage / 64;

    private static Texture2D frameTexture;

    public bool highlight = false;
    public readonly bool valid = true;

    public bool IsPicked {get; private set;} = false;
    public Rectangle Bounds {get {return this.drawArea;}}
    public readonly int index;

    Rectangle drawArea;
    Rectangle textureDrawArea;
    private const int numberOfOptions = 2;

    private string text;

    private static readonly Color frameColor = ColorConfig.pallet0[2];
    private static readonly Color fillColor = ColorConfig.pallet0[0];
    private static readonly Color textColor = Color.Black;

    

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
        int innerBufferPxSoze = innerWidthBufferPxSize > innerHeightBufferPxSize ? innerWidthBufferPxSize : innerHeightBufferPxSize;

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
            drawArea.X + innerBufferPxSoze,
            drawArea.Y + innerBufferPxSoze,
            drawArea.Width - innerBufferPxSoze * 2,
            drawArea.Height - innerBufferPxSoze * 2
        );

    }

    public void Draw()
    {
        GameWindow.spriteBatchUi.Draw(GameWindow.whitePixelTexture, this.drawArea, frameColor);
        GameWindow.spriteBatchUi.Draw(GameWindow.whitePixelTexture, this.textureDrawArea, fillColor);

        int fontSize = 86;
        SpriteFontBase font = ResourcesUi.FontSystem.GetFont(fontSize);
        Vector2 centerOfButtonVec = this.textureDrawArea.Center.ToVector2();
        Vector2 fontVec = new(font.MeasureString(text).Length() / 2, fontSize / 2);
        Vector2 vec = centerOfButtonVec - fontVec;
        GameWindow.spriteBatchUi.DrawString(font, this.text, vec, textColor);   
    }

}