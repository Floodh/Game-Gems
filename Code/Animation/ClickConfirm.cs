using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class ClickConfirm : Animation
{
    private static Texture2D[] textures;
    public ClickConfirm(Rectangle worldArea)
        : this(worldArea.Location, worldArea.Width)
    {
        if (worldArea.Width != worldArea.Height)
            throw new Exception("ClickConfirm area width and height can not be different when using this constructor.");
    }
    public ClickConfirm(Point worldPosition, int size)
        : base(RenderFrames(worldPosition, size), 3, false)
    {}

    private static Tuple<Texture2D[], Rectangle> RenderFrames(Point worldPosition, int size)
    {
        if(textures == null)
            textures = TextureSource.LoadClickConfirm();
        Rectangle drawArea = new Rectangle(worldPosition.X, worldPosition.Y, size, size);
        return new Tuple<Texture2D[], Rectangle>(textures, drawArea);
    }

}