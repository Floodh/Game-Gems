using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class Wall : UpgradeableBuilding
{

    public static readonly Resources[] costs = new Resources[]
    {
        new Resources(32,0,0,0),
        new Resources(64,0,0,0),
        new Resources(128,0,0,0),
        new Resources(256,0,0,0),
        new Resources(512,0,0,0),
        new Resources(1024,0,0,0),
    };
    private static readonly int[] maxHealth = new int[]
    {
        400,
        800,
        1600,
        3200,
        6400,
    };

    private const int textureSet = 0;


    public Wall()
        : base("walls2", textureSet)
    { }

    public override void Tick()
    {
        base.Tick();
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            Rectangle rect = new(DrawArea.Location, DrawArea.Size);
            GameWindow.spriteBatch.Draw(baseTextures[textureSet][CurrentTier], rect, Sunlight.Mask);
            hpBar.Draw();
        }
    }

    protected override void UpdateStats()
    {
        this.MaxHp = maxHealth[CurrentTier];
        this.Hp = this.MaxHp;
    }
    public override Resources? GetUpgradeCost()
    {
        if (CurrentTier >= this.MaxTierLevel - 1)
            return null;
        else
            return costs[CurrentTier];
    }
    public static new Building CreateNew()
    {
        return new Wall();
    }

    public static new Building Buy()
    {
        if (Resources.BuyFor(costs[0]))
            return CreateNew();
        else
            return null;
    }

    public static new Texture2D[] GetTextures()
    {
        return baseTextures[textureSet];
    }

    public static new Rectangle GetRectangle(Point point)
    {
        return new Rectangle(point.X, point.Y, Map.mapPixelToTexturePixel_Multiplier * 2, Map.mapPixelToTexturePixel_Multiplier * 2);
    }

    public override string ToString()
    {
        return $"Wall : {this.Hp} / {this.MaxHp}";
    }
}