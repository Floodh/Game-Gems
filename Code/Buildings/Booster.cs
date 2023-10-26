using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


class Booster : UpgradeableBuilding
{
    private const int textureSet = 3;


    public Booster()
        : base("Purple", textureSet)
    {
        this.MaxEnergy = 100;
        this.Energy = 100;
        this.Regen_Energy = 0;

        for (int tier = 0; tier < maxTier; tier++)
            Console.WriteLine($"textureSet:{textureSet}, tier:{tier}");

        for (int tier = 0; tier < maxTier; tier++)
                baseTextures[textureSet][tier] = Texture2D.FromFile(GameWindow.graphicsDevice, $"Data/TextureSources/income-tower3-tier{tier+1}.png");
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

    public override Building CreateNew()
    {
        return new Booster();
    }
    
    public override string ToString()
    {
        return $"Booster : {this.Hp} / {this.MaxHp} / tier:{this.Tier}";
    }

}