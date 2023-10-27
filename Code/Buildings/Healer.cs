using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class Healer : UpgradeableBuilding
{

    private const int textureSet = 1;


    private Targetable target;
    private EnergyBeam animation;
    private EnergyBar energyBar;
    
    private int dmg = -1;
  


    public Healer()
        : base("healing-tower1", textureSet)
    {
        this.AttackRate = 10;
        this.MaxEnergy = 100; 
        this.energyBar = new EnergyBar(this);
        //animation = new(this.GridArea.Center, new Point(40,50), EnergyBeam.Type.Line); 
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
                _ = new Projectile(dmg, 0, 100000f, target, this);
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

    public static new Building CreateNew()
    {
        return new Healer();
    }
    public override string ToString()
    {
        return $"Healer : Hp:{this.Hp}/{this.MaxHp}, Energy:{this.Energy}/{this.MaxEnergy}";
    }
}