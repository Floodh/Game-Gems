using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


class Booster : UpgradeableBuilding
{
    private const int textureSet = 4;


    public Booster()
        : base("income-tower3", textureSet)
    {
        this.MaxEnergy = 100;
        this.Energy = 100;
        this.Regen_Energy = 0;
    }

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
            GameWindow.spriteBatch.Draw(baseTextures[textureSet][currentTier-1], Camera.ModifiedDrawArea(rect, Camera.zoomLevel), Sunlight.Mask);
            hpBar.Update();
            hpBar.Draw();
        }
      }

    public static new Building CreateNew()
    {
        return new Booster();
    }
    
    public override string ToString()
    {
        return $"Booster : {this.Hp} / {this.MaxHp} / tier:{this.Tier}";
    }

}