using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


class Cannon : UpgradeableBuilding
{
    private const int damageMultipler = 10;

    public static readonly Resources[] costs = new Resources[]
    {
        new Resources(8,0,32,0),
        new Resources(16,0,64,0),
        new Resources(32,0,128,0),
        new Resources(64,0,256,0),
        // new Resources(128,0,512,0),
        // new Resources(256,0,1024,0),
    };
    private static readonly int[] dmg = new int[]
    {
        5,
        5,
        5,
        5,
        // 10,
        // 20,
        // 40,
        // 80,
        // 160,
    };
    private static readonly int[] attackTickCost = new int[]
    {
        80,
        40,
        20,
        10,
    };    
    private static readonly int[] maxHealth = new int[]
    {
        100,
        200,
        400,
        800,
        // 1600,
    };
    private static readonly int[] maxEnergy = new int[]
    {
        100,
        200,
        400,
        800,
        // 1600,
    };

    private static readonly int[] emitterOffset = new int[]
    {
        29,
        50,
        81,
        114,
    };

    private const int textureSet = 3;


    private EnergyBar energyBar;
    private Targetable target = null;
    private readonly int range = 1200;

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
            if (initative++ > this.AttackRate)
                if (this.Energy >= dmg[CurrentTier])
                    if (distanceSquared > this.range * this.range)
                    {
                        this.target = null;
                        initative = this.AttackRate / 2;
                    }
                    else
                    {
                        Vector2 sourceVec = this.TargetPosition.ToVector2() + new Vector2(0, -emitterOffset[CurrentTier]);
                        this.Energy -= dmg[CurrentTier];
                        _ = new Projectile(dmg[CurrentTier] * damageMultipler, 0, 3.13f, this.target, sourceVec, 2, 5, this.OnHit);
                        initative = 0;
                        if (this.target.IsDead)
                            this.target = null;
                    }

        }
        else
        {
            this.target = FindTargetUnit(this, Faction.Enemy, false, false);
        }

        this.energyBar.Update();
    }

    private void OnHit(Vector2 impactLocation)
    {
        float radius = 10;
        Vector2 vec = impactLocation - new Vector2(radius, radius);
        new Explosion(vec.ToPoint(), Convert.ToInt16(radius)).Play();
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            Rectangle rect = new(DrawArea.X + 32, DrawArea.Y - 8 - 64, DrawArea.Width / 2, DrawArea.Height / 2 * 3);
            GameWindow.spriteBatch.Draw(baseTextures[textureSet][CurrentTier], rect, Sunlight.Mask);
            hpBar.Draw();
        }
        this.energyBar.Draw();
    }

    public override bool TryUpgrade()
    {
        bool uppgraded = base.TryUpgrade();
        if (uppgraded)
            UpdateStats();
        return uppgraded;
    }

    protected override void UpdateStats()
    {
        this.AttackRate = attackTickCost[CurrentTier];
        this.MaxEnergy = maxEnergy[CurrentTier];
        this.MaxHp = maxHealth[CurrentTier];
        this.AttackDmg = dmg[CurrentTier];
        this.Hp = this.MaxHp;
        this.Energy = this.MaxEnergy;
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
        return new Cannon();
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
        return new Rectangle(point.X + 32, point.Y - 8 - 64, Map.mapPixelToTexturePixel_Multiplier, Map.mapPixelToTexturePixel_Multiplier * 3);
    }

    public override string ToString()
    {
        return $"Cannon : {this.Hp} / {this.MaxHp} / tier:{this.Tier}";
    }

}