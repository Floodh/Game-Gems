using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


class Cannon : UpgradeableBuilding
{

    public static readonly Resources[] costs = new Resources[]
    {
        new Resources(16,0,64,0),
        new Resources(32,0,128,0),
        new Resources(64,0,256,0),
        new Resources(128,0,512,0),
        new Resources(256,0,1024,0),
    };
    private static readonly int[] dmg = new int[]
    {
        10,
        20,
        40,
        80,
        160,
    };
    private static readonly int[] maxHealth = new int[]
    {
        100,
        200,
        400,
        800,
        1600,
    };
    private static readonly int[] maxEnergy = new int[]
    {
        100,
        200,
        400,
        800,
        1600,
    };

    private const int textureSet = 3;


    private EnergyBar energyBar;
    private Targetable target = null;
    private readonly int range = 400;

    private readonly int requiredInitative = 80;
    private int initative = 0;

    public Cannon()
        : base("attack-tower1", textureSet)
    {
        this.energyBar = new EnergyBar(this);
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
            if (this.Energy >= dmg[currentTierIndex])
            if (distanceSquared > this.range * this.range)
            {
                this.target = null;
                initative = requiredInitative / 2;
            }
            else
            {
                this.Energy -= dmg[currentTierIndex];
                _ = new Projectile(dmg[currentTierIndex], 0, 3.13f, this.target, this, 2);
                initative = 0;
            }
        }
        else
        {
            this.target = FindTargetUnit(this, Faction.Enemy, false, false);
        }

        this.energyBar.Update();
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
        this.energyBar.Draw();
    }

    public override bool TryUpgrade()
    {
        bool uppgraded = base.TryUpgrade();
        if (uppgraded)
            UppdateStats();
        return uppgraded;
    }

    protected override void UppdateStats()
    {
        this.MaxEnergy = maxEnergy[currentTierIndex];
        this.MaxHp = maxHealth[currentTierIndex];
        this.AttackDmg = dmg[currentTierIndex];
        this.Hp = this.MaxHp;
        this.Energy = this.MaxEnergy;        
    }
    public override Resources GetUpgradeCost()
    {
        return costs[currentTierIndex+1];
    }    
    public static new Building CreateNew()
    {
        return new Cannon();
    }

    public static new Texture2D[] GetTextures()
    {
        return baseTextures[textureSet];
    }

    public static new Rectangle GetRectangle(Point point)
    {
        int mapPixelToTexturePixel_Multiplier = Map.mapPixelToTexturePixel_Multiplier;
        return new Rectangle(point.X+32, point.Y-8-64, mapPixelToTexturePixel_Multiplier, mapPixelToTexturePixel_Multiplier*3);
    }
    
    public override string ToString()
    {
        return $"Cannon : {this.Hp} / {this.MaxHp} / tier:{this.Tier}";
    }

}