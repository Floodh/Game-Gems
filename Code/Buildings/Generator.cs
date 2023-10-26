using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class Generator : UpgradeableBuilding
{

    private const int textureSet = 2;

    private Targetable target;
    private EnergyBeam animation;
    private EnergyBar energyBar;

    private int energyTransfer = 1;

    public Generator()
        : base("Orange", textureSet)
    {
        this.AttackRate = 10;
        this.energyBar = new EnergyBar(this);
        this.MaxEnergy = 1;
        this.Energy = 1;
        this.Regen_Energy = 0;

        for (int tier = 0; tier < maxTier; tier++)
            Console.WriteLine($"textureSet:{textureSet}, tier:{tier}");

        for (int tier = 0; tier < maxTier; tier++)
                baseTextures[textureSet][tier] = Texture2D.FromFile(GameWindow.graphicsDevice, $"Data/TextureSources/energy-tower1-tier{tier+1}.png");
    }

    int attackCounter = 0;
    public override void Tick()
    {
        base.Tick();
        if (this.target == null || this.target.Energy == this.target.MaxEnergy)
        {
            this.target = this.FindTarget(this, Faction.Player, false, true);
            if (this.target != null)
                animation = new(this.GridArea.Location, target.GridArea.Location, EnergyBeam.Type.Line); 
        }
        else
        {
            attackCounter++;
            if (attackCounter >= AttackRate)
            {          
                //Console.WriteLine($"Giving energy to : {target}");
                Projectile projectile = new Projectile(0, energyTransfer, 100000f, target, this);
                attackCounter = 0;
                if (this.animation.IsPlaying == false)
                    this.animation.Play();
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
            GameWindow.spriteBatch.Draw(baseTextures[textureSet][currentTier-1], Camera.ModifiedDrawArea(rect, Camera.zoomLevel), Sunlight.Mask);
            hpBar.Update();
            hpBar.Draw();
        }
        // base.Draw();

        this.energyBar.Draw();
    }

    public override Building CreateNew()
    {
        return new Generator();
    }
    public override string ToString()
    {
        return $"Generator : {this.Hp} / {this.MaxHp}";
    }
}