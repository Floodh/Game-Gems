using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


class Generator : UpgradeableBuilding
{

    private const int textureSet = 2;

    private Targetable target;
    private EnergyBeam animation;
    private int energyTransfer = 1;


    public Generator()
        : base("Orange", textureSet)
    {
        this.AttackRate = 10;
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
                Projectile projectile = new Projectile(0, energyTransfer, target, this);
                attackCounter = 0;
                if (this.animation.IsPlaying == false)
                    this.animation.Play();
            }
        }
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