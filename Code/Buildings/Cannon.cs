using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


class Cannon : UpgradeableBuilding
{
    private const int textureSet = 3;


    private EnergyBar energyBar;
    private Targetable target = null;
    private readonly int range = 400;
    private readonly int dmg = 10;

    private readonly int requiredInitative = 80;
    private int initative = 0;

    public Cannon()
        : base("Purple", textureSet)
    {
        this.energyBar = new EnergyBar(this);
        this.MaxEnergy = 100;
        this.Energy = 100;
        this.Regen_Energy = 0;

        for (int tier = 0; tier < maxTier; tier++)
            baseTextures[textureSet][tier] = Texture2D.FromFile(GameWindow.graphicsDevice, $"Data/TextureSources/attack-tower1-tier{tier+1}.png");
    }

    public override void Tick()
    {
        base.Tick();

        //
        if (this.target != null)
        {
            int dx = this.target.TargetPosition.X - this.TargetPosition.X;
            int dy = this.target.TargetPosition.X - this.TargetPosition.X;
            int distanceSquared = dx * dx + dy * dy;
            if (initative++ > requiredInitative)
            if (distanceSquared > this.range * this.range)
            {
                this.target = null;
                initative = requiredInitative / 2;
            }
            else
            {
                _ = new Projectile(this.dmg, 0, 3.13f, this.target, this, 2);
                initative = 0;
            }
        }
        else
        {
            this.target = FindTarget(this, Faction.Enemy, false, false);
        }

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

    public override Building CreateNew()
    {
        return new Cannon();
    }
    
    public override string ToString()
    {
        return $"Cannon : {this.Hp} / {this.MaxHp} / tier:{this.Tier}";
    }

}