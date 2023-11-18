

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

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

    public Color GemColor
    {
        get {
            if (type == Type.Blue)
                return Color.Blue;
            else if (type == Type.Green)
                return Color.Green;
            else if (type == Type.Purple)
                return Color.Purple;
            else if (type == Type.Orange)
                return Color.Orange;
            else
                throw new Exception("Can't get a Color since the Mineral does not have an assigned type.");
        }
    }

    private static Texture2D[] baseTextures;

    public int quantity = 10000;
    
    public Mineral(Type type)
        : base(Faction.Neutral)
    {
        this.type = type;
        Rectangle animationArea = this.GridArea;
        //this.numberAnimation = new Explosion(new(DrawArea.X+8, DrawArea.Y-4, DrawArea.Width-16, DrawArea.Height-16));

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

    public static new Building CreateNew()
    {
        throw new NotImplementedException();
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

        // if (this.numberAnimation == null)
        // {
            
        // }

    }
}