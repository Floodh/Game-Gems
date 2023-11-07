using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


class Booster : UpgradeableBuilding
{

    public static readonly Resources[] costs = new Resources[]
    {
        new Resources(64,64,64,64),
        new Resources(128,128,128,128),
        new Resources(256,256,256,256),
        new Resources(512,512,512,512),
        new Resources(1024,1024,1024,1024),
    };    
    private static readonly int[] maxHealth = new int[]
    {
        100,
        200,
        400,
        800,
        1600,
    };

    private const int textureSet = 4;


    public Booster()
        : base("income-tower3", textureSet)
    {}

    public override void Tick()
    {
        base.Tick();
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            Rectangle rect = new(DrawArea.X+32, DrawArea.Y-8-64, DrawArea.Width/2, DrawArea.Height/2*3);
            GameWindow.spriteBatch.Draw(baseTextures[textureSet][currentTierIndex], Camera.ModifiedDrawArea(rect, Camera.zoomLevel), Sunlight.Mask);
            hpBar.Update();
            hpBar.Draw();
        }
    }
    protected override void UpdateStats()
    {
        this.MaxHp = maxHealth[currentTierIndex];
        this.Hp = this.MaxHp;    
    }
    public override Resources GetUpgradeCost()
    {
        return costs[currentTierIndex];
    }    
    public static new Building CreateNew()
    {
        return new Booster();
    }

    public static new Building Buy()
    {
        if(Resources.BuyFor(costs[0]))
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
        return new Rectangle(point.X+32, point.Y-8-64, Map.mapPixelToTexturePixel_Multiplier, Map.mapPixelToTexturePixel_Multiplier*3);
    }
   
    public override string ToString()
    {
        return $"Booster : {this.Hp} / {this.MaxHp} / tier:{this.Tier}";
    }

}