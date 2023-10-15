using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class Healer : Building
{

    private const string Path_BaseTexture = "Data/Texture/Healer.png";

    private Texture2D baseTexture;
    private HealthBar hpBar;
    private EnergyBar energyBar;
    private Targetable target;
    private EnergyBeam animation;
    
    private int dmg = -1;
    


    public Healer()
        : base(Faction.Player)
    {
        this.AttackRate = 10;
        this.MaxEnergy = 100;

        this.baseTexture = Texture2D.FromFile(GameWindow.graphicsDevice, Path_BaseTexture);
        hpBar = new HealthBar(this);
        energyBar = new EnergyBar(this);     
        //animation = new(this.GridArea.Center, new Point(40,50), EnergyBeam.Type.Line);   
    }

    public override void Draw()
    {
        Rectangle gridArea = this.GridArea;
        if (gridArea != Rectangle.Empty)
        {
            GameWindow.spriteBatch.Draw(baseTexture, Camera.ModifiedDrawArea(DrawArea, Camera.zoomLevel), Sunlight.Mask);
            hpBar.Update();
            energyBar.Update();
            hpBar.Draw();
            energyBar.Draw();
        }
        base.Draw();
    }

    int attackCounter = 0;
    public override void Tick()
    {
        base.Tick();
        if (this.target == null || this.target.Hp == this.target.MaxHp || this.target.IsDead)
        {
            this.target = this.FindTarget(this, Faction.Player, true, false);
            if (this.target != null)
                animation = new(this.GridArea.Location, target.GridArea.Location, EnergyBeam.Type.Line); 
        }
        else
        {
            attackCounter++;
            if (attackCounter >= AttackRate)
            if (this.Energy >= -dmg)
            {           
                //Console.WriteLine("Healig");
                Projectile projectile = new(dmg, 0, target, this);
                attackCounter = 0;
                if (this.animation.IsPlaying == false)
                    this.animation.Play();
            }

        }
        
    }

    public override Building CreateNew()
    {
        return new Healer();
    }
    public override string ToString()
    {
        return $"Healer : Hp:{this.Hp}/{this.MaxHp}, Energy:{this.Energy}/{this.MaxEnergy}";
    }
}