using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class Generator : UpgradeableBuilding
{

    public static readonly Resources[] costs = new Resources[]
    {
        new Resources(16,0,0,64),
        new Resources(32,0,0,128),
        new Resources(64,0,0,256),
        // new Resources(128,0,0,512),
        // new Resources(256,0,0,1024),
    };
    private static readonly int[] maxHealth = new int[]
    {
        100,
        200,
        400,
        800,
        // 1600,
    };
    private static readonly int[] energyTransfer = new int[]
    {   //  keep in mind that the actual transfered energy is trippled
        10,
        20,
        40,
        80,
        // 160,
    };

    private static readonly int[] emitterOffset = new int[]
    {
        29,
        50,
        81,
        114,
    };

    private const int textureSet = 2;

    private Targetable target;
    private EnergyBar energyBar;


    public Generator()
        : base("energy-tower1", textureSet)
    {
        this.energyBar = new EnergyBar(this);
        this.MaxEnergy = 1;
        this.Energy = 1;
        this.AttackRate = 10;
    }

    int attackCounter = 0;
    public override void Tick()
    {
        base.Tick();
        if (this.target == null || this.target.Energy == this.target.MaxEnergy)
        {
            this.target = this.FindTarget(this, Faction.Player, false, true);
        }
        else
        {
            attackCounter++;
            if (attackCounter >= AttackRate)
            {
                //Console.WriteLine($"Giving energy to : {target}");
                Vector2 sourceVec = this.TargetPosition.ToVector2() + new Vector2(0, -emitterOffset[CurrentTier]);
                Projectile projectile = new Projectile(0, energyTransfer[CurrentTier], 4f, target, sourceVec, 4, 5);
                projectile.Rotate = false;
                projectile.Scale = 0.075f;

                attackCounter = 0;
            }
        }

        this.energyBar.Update();
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
        return new Generator();
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
        return $"Generator : {this.Hp} / {this.MaxHp}";
    }
}