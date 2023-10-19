using System;
using Microsoft.Xna.Framework;

abstract class Targetable
{

    public int MaxHp {get; protected set;} = 100;
    protected int StartHp {get; set;} = 100;
    protected int MaxSheild {get; set;} = 0;
    protected int StartSheild {get; set;} = 0;
    public int MaxEnergy {get; protected set;} = 0;
    protected int StartEnergy {get; set;} = 0;

    protected int DmgReduction_Health {get; set;} = 0;
    protected int DmgReduction_Sheild {get; set;} = 0;

    protected int Regen_Health {get; set;} = 0; //  per tick
    protected int Regen_Sheild {get; set;} = 1;
    protected int Regen_Energy {get; set;} = 0;

    protected int AttackDmg {get; set;} = 1;
    protected int AttackRate {get; set;} = 50; //  lower = faster

    
    public int Hp {get; protected set;}
    protected int sheild;
    public int Energy {get; protected set;}

    public abstract Point TargetPosition {get;}
    public virtual Rectangle GridArea {get; protected set;} //  should not be used for units (yet)

    public readonly Faction faction;

    public bool IsDead{get; protected set;} = false;

    public Targetable(Faction faction)
    {
        this.faction = faction;
        this.Hp = StartHp;
        this.sheild = StartSheild;
        this.Energy = StartEnergy;
    }

    public virtual void Tick()
    {

        this.Hp += Regen_Health;
        this.sheild += Regen_Sheild;
        this.Energy += Regen_Energy;

        this.Hp = Math.Min(this.Hp, MaxHp);
        this.sheild = Math.Min(this.sheild, MaxSheild);
        this.Energy = Math.Min(this.Energy, MaxEnergy);
        if (this.Hp <= 0)
            this.Die();

    }
    
    public abstract bool Hit(Projectile projectile);

    public void TakeDmg(Projectile projectile)
    {
        if (projectile.damage < 0)
        {
            this.Hp -= projectile.damage;
            this.Hp = Math.Min(Hp, MaxHp);
            return;
        }
        if (projectile.energyTransfer > 0)
        {
            TakeDmg_Energy(projectile.energyTransfer);
            return;
        }

        int dmg = projectile.damage;
        dmg = TakeDmg_Sheild(dmg);
        if (dmg > 0)
            TakeDmg_Health(dmg);

    
    }

    private int TakeDmg_Sheild(int dmg)
    {
        if (this.sheild <= 0)
            return dmg;

        dmg -= this.DmgReduction_Sheild;
        dmg = Math.Max(dmg, 1);
        this.sheild -= dmg;
        this.DmgReduction_Health = -sheild;

        if (this.sheild < 0)
        {
            this.sheild = 0;
            return -dmg;
        }
        else
            return 0;
   
    }

    private void TakeDmg_Health(int dmg)
    {
        dmg -= this.DmgReduction_Health;
        dmg = Math.Max(dmg, 1);
        this.Hp -= dmg;     
    }

    private void TakeDmg_Energy(int energy)
    {
        this.Energy += energy;
        this.Energy = Math.Min(this.Energy, MaxEnergy);
    }

    protected abstract void Die();

    public virtual void PlayerInteraction()
    {
        
    }
    
    protected Targetable FindTarget(Targetable self, Faction faction, bool inNeed_Health, bool inNeed_Energy)
    {
        double distanceSquared = double.MaxValue;
        Targetable target = null;

        foreach (Building building in Building.allBuildings)
            if (building.faction == Faction.Player)
            if (!inNeed_Health || building.Hp != building.MaxHp)
            if (!inNeed_Energy || building.Energy != building.MaxEnergy)
        {

            Point diffP = this.TargetPosition - building.TargetPosition;
            double newDistance = (diffP.X * diffP.X) + (diffP.Y * diffP.Y);

            if (newDistance < distanceSquared)
            {
                distanceSquared = newDistance;
                target = building;
            }

        }

        foreach (Unit unit in Unit.allUnits)
            if (unit.faction == Faction.Player)
            if (!inNeed_Health || unit.Hp != unit.MaxHp)
            if (!inNeed_Energy || unit.Energy != unit.MaxEnergy)
        {

            Point diffP = this.TargetPosition - unit.TargetPosition;
            double newDistance = diffP.X * diffP.X + diffP.Y * diffP.Y;

            if (newDistance < distanceSquared)
            {
                distanceSquared = newDistance;
                target = unit;
            }

        }

        if (target == self)
            return null;
        return target;

    }

    
}