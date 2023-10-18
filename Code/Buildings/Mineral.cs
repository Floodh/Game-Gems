

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Mineral : Building
{

    public new enum Type
    {
        Blue = 0,
        Green = 1,
        Purple = 2,
        Red = 3,
    }
    public readonly Type type;
    private static Texture2D[] baseTextures;

    public int quantity = 10000;

    public Mineral(Type type)
        : base(Faction.Neutral)
    {
        this.type = type;
        baseTextures ??= TextureSource.LoadMinerals();
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            GameWindow.spriteBatch.Draw(baseTextures[((int)this.type)], Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Sunlight.Mask);
        }
        base.Draw();
    }

    public override Building CreateNew()
    {
        return new Mineral(this.type);
    }

    public override void Tick()
    {
        if (this.quantity == 0)
            this.Die();
    }

}