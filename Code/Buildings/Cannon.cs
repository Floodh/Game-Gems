using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


class Cannon : UpgradeableBuilding
{
    private const int textureSet = 3;

    private EnergyBar energyBar;

    public Cannon()
        : base("attack-tower1", textureSet)
    {
        this.energyBar = new EnergyBar(this);
        this.MaxEnergy = 100;
        this.Energy = 100;
        this.Regen_Energy = 0;

        for (int tier = 0; tier < maxTier; tier++)
            Console.WriteLine($"textureSet:{textureSet}, tier:{tier}");

        for (int tier = 0; tier < maxTier; tier++)
                baseTextures[textureSet][tier] = Texture2D.FromFile(GameWindow.graphicsDevice, $"Data/TextureSources/attack-tower1-tier{tier+1}.png");
    }

    public override void Tick()
    {
        base.Tick();
        this.energyBar.Update();
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
        // base.Draw();

        this.energyBar.Draw();
    }

    public static new Building CreateNew()
    {
        return new Cannon();
    }
    
    public override string ToString()
    {
        return $"Cannon : {this.Hp} / {this.MaxHp} / tier:{this.Tier}";
    }

}