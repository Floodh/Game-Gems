using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class Healer : UpgradeableBuilding
{

    public static readonly Resources[] costs = new Resources[]
    {
        new Resources(16,64,0,0),
        new Resources(32,128,0,0),
        new Resources(64,256,0,0),
        // new Resources(128,512,0,0),
        // new Resources(256,1024,0,0),
    };

    private static readonly int[] healing = new int[]
    {
        10,
        20,
        40,
        80,
        // 160,
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

    private const int textureSet = 1;


    private Targetable target;

    private EnergyBar energyBar;

    public Healer()
        : base("healing-tower1", textureSet)
    {
        this.energyBar = new EnergyBar(this);
        this.AttackRate = 10;
    }

    int attackCounter = 0;
    public override void Tick()
    {
        base.Tick();
        if (this.target == null || this.target.Hp == this.target.MaxHp || this.target.IsDead)
        {
            this.target = this.FindTarget(this, Faction.Player, true, false);

        }
        else
        {
            attackCounter++;
            if (attackCounter >= AttackRate)
                if (this.Energy >= healing[CurrentTier])
                {
                    Vector2 sourceVec = this.TargetPosition.ToVector2() + new Vector2(0, -emitterOffset[CurrentTier]);
                    this.Energy -= healing[CurrentTier];
                    Projectile projectile = new(-healing[CurrentTier], 0, 4f, target, sourceVec, 3, 5);
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
        this.MaxEnergy = maxEnergy[CurrentTier];
        this.MaxHp = maxHealth[CurrentTier];
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
        return new Healer();
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
        return $"Healer : Hp:{this.Hp}/{this.MaxHp}, Energy:{this.Energy}/{this.MaxEnergy}";
    }
}