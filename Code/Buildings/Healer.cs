using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class Healer : Building
{

    private const string Path_BaseTexture = "Data/Texture/Healer.png";

    Texture2D baseTexture;
    private HealthBar hpBar;
    private Targetable target;

    private int dmg = -10;

    public Healer()
        : base(Faction.Player)
    {
        this.AttackRate = 20;
        this.MaxEnergy = 100;

        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
        hpBar = new HealthBar(this);
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            GameWindow.spriteBatch.Draw(baseTexture, Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Color.White);
            hpBar.update();
            hpBar.Draw();
        }
        base.Draw();
    }

    int attackCounter = 0;
    public override void Tick()
    {
        base.Tick();
        if (this.target == null || this.target.Hp == this.target.MaxHp)
        {
            this.target = this.FindTarget(Faction.Player, true, false);
        }
        else
        {
            //Console.WriteLine($"Healer energy {Energy}");
            attackCounter++;
            if (attackCounter >= AttackRate)
            if (this.Energy >= -dmg)
            {           
                //Console.WriteLine("Healig");
                Projectile projectile = new Projectile(dmg, 0, target, this);
                attackCounter = 0;
            }
        }
        
    }
    public override string ToString()
    {
        return $"Healer : Hp:{this.Hp}/{this.MaxHp}, Energy:{this.Energy}/{this.MaxEnergy}";
    }
}