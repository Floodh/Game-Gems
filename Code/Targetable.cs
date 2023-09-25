using System;
using Microsoft.Xna.Framework;

abstract class Targetable
{

    public int MaxHp {get; protected set;} = 100;
    protected int StartHp {get; set;} = 100;
    protected int MaxSheild {get; set;} = 0;
    protected int StartSheild {get; set;} = 0;

    protected int DmgReduction_Health {get; set;} = 0;
    protected int DmgReduction_Sheild {get; set;} = 0;

    protected int Regen_Health {get; set;} = 0; //  per tick
    protected int Regen_Sheild {get; set;} = 1;

    protected int AttackDmg {get; set;} = 1;
    protected int AttackRate {get; set;} = 100; //  lower = faster

    
    public int Hp {get; protected set;}
    protected int sheild;

    public abstract Point TargetPosition {get;}

    public readonly Faction faction;

    public Targetable(Faction faction)
    {
        this.faction = faction;
        this.Hp = StartHp;
        this.sheild = StartSheild;
    }

    public virtual void Tick()
    {

        this.Hp += Regen_Health;
        this.sheild += Regen_Sheild;

        this.Hp = Math.Min(this.Hp, MaxHp);
        this.sheild = Math.Min(this.sheild, MaxSheild);
        if (this.Hp <= 0)
            this.Die();

    }
    
    public abstract bool Hit(Projectile projectile);

    public void TakeDmg(Projectile projectile)
    {
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

    protected abstract void Die();


    
}