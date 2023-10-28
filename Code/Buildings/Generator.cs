using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class Generator : UpgradeableBuilding
{

    private static readonly Resources[] costs = new Resources[]
    {
        new Resources(16,0,0,64),
        new Resources(32,0,0,128),
        new Resources(64,0,0,256),
        new Resources(128,0,0,512),
        new Resources(256,0,0,1024),
    };
    private static readonly int[] maxHealth = new int[]
    {
        100,
        200,
        400,
        800,
        1600,
    };
    private static readonly int[] energyTransfer = new int[]
    {   //  keep in mind that the actual transfered energy is trippled
        10,
        20,
        40,
        80,
        160,
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
                Projectile projectile = new Projectile(0, energyTransfer[currentTierIndex], 4f, target, this, 4);
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
            Rectangle rect = new(DrawArea.X+32, DrawArea.Y-8-64, DrawArea.Width/2, DrawArea.Height/2*3);
            GameWindow.spriteBatch.Draw(baseTextures[textureSet][currentTierIndex], Camera.ModifiedDrawArea(rect, Camera.zoomLevel), Sunlight.Mask);
            hpBar.Update();
            hpBar.Draw();
        }
        // base.Draw();

        this.energyBar.Draw();
    }

    protected override void UppdateStats()
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
        return new Generator();
    }
    public override string ToString()
    {
        return $"Generator : {this.Hp} / {this.MaxHp}";
    }
}