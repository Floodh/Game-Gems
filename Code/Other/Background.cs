
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Background
{

    private Rectangle DrawArea {get {return new Rectangle(0,0,windowSize.X, windowSize.Y);}}
    public Point windowSize;
    private Texture2D texture;



    public Background(Point windowSize, GraphicsDevice graphicsDevice)
    {
        this.windowSize = windowSize;
        this.texture = Texture2D.FromFile(graphicsDevice, "Data/Texture/Background.jpg");
    }
    public Background(int width, int height, GraphicsDevice graphicsDevice)
        : this(new Point(width, height), graphicsDevice)
    {}

    public void Draw()
    {
        GameWindow.spriteBatchUi.Draw(this.texture, DrawArea, Sunlight.Mask);
    }

}