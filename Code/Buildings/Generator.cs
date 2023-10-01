using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class Generator : Building
{

    private const string Path_BaseTexture = "Data/Texture/Generator.png";

    Texture2D baseTexture;
    private HealthBar hpBar;
    private Targetable target;

    private int energyTransfer = 1;


    public Generator()
        : base(Faction.Player)
    {
        this.AttackRate = 10;

        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
        hpBar = new HealthBar(this);
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            GameWindow.spriteBatch.Draw(baseTexture, Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Color.White);
            hpBar.Update();
            hpBar.Draw();
        }
        base.Draw();
    }

    int attackCounter = 0;
    public override void Tick()
    {
        base.Tick();
        if (this.target == null || this.target.Energy == this.target.MaxEnergy)
        {
            this.target = this.FindTarget(Faction.Player, false, true);
        }
        else
        {
            attackCounter++;
            if (attackCounter >= AttackRate)
            {          
                Console.WriteLine($"Giving energy to : {target}");
                Projectile projectile = new Projectile(0, energyTransfer, target, this);
                attackCounter = 0;
            }
        }        
    }
    public override string ToString()
    {
        return $"Generator : {this.Hp} / {this.MaxHp}";
    }
}