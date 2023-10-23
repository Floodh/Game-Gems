

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Mineral : Building
{

    public enum Type
    {
        Blue = 0,
        Green = 1,
        Purple = 2,
        Orange = 3,
    }
    public readonly Type type;
    private static Texture2D[] baseTextures;

    public int quantity = 10000;
    
    public Mineral(Type type)
        : base(Faction.Neutral)
    {
        this.type = type;
        if (baseTextures == null)
        {
            baseTextures = TextureSource.LoadMinerals();       
        }
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            Rectangle mineralRect = new(DrawArea.X+8, DrawArea.Y-4, DrawArea.Width-16, DrawArea.Height-16);
            GameWindow.spriteBatch.Draw(baseTextures[((int)this.type)], Camera.ModifiedDrawArea(mineralRect, Camera.zoomLevel), Sunlight.Mask);
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

    public override void PlayerInteraction()
    {
        base.PlayerInteraction();

        switch (this.type)
        {
            case Type.Blue:
                Resources.Gain(1,0,0,0);
                break;
            case Type.Green:
                Resources.Gain(0,1,0,0);
                break;
            case Type.Purple:
                Resources.Gain(0,0,1,0);
                break;
            case Type.Orange:
                Resources.Gain(0,0,0,1);
                break;                                
        }

    }
}