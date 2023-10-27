using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Explosion : Animation
{
    public Explosion(Rectangle worldArea)
        : this(worldArea.Location, worldArea.Width)
    {
        if (worldArea.Width != worldArea.Height)
            throw new Exception("Explosion area width and height can not be different when using this constructor.");
    }
    public Explosion(Point worldPosition, int size)
        : base(RenderFrames(worldPosition, size), 3)
    {}

    private static Tuple<Texture2D[], Rectangle> RenderFrames(Point worldPosition, int size)
    {
        Texture2D[] textures = TextureSource.LoadExplosion();
        Rectangle drawArea = new Rectangle(worldPosition.X, worldPosition.Y, size, size);
        return new Tuple<Texture2D[], Rectangle>(textures, drawArea);
    }

}