using System;
using Microsoft.Xna.Framework;

abstract class Targetable
{

    protected const int maxHp = 100;
    protected const int startHp = 100;
    protected const int maxSheild = 0;
    protected const int startSheild = 0;

    protected const int baseDmgReduction_Health = 0;
    protected const int baseDmgReduction_Sheild = 0;

    protected const int regen_Health = 0; //  per tick
    protected const int regen_Sheild = 1;

    protected const int attackDmg = 1;
    protected const int attackRate = 100; //  lower = faster

    
    protected int hp = startHp;
    protected int sheild = startSheild;

    protected int dmgReduction_Health = baseDmgReduction_Health;
    protected int dmgReduction_Sheild = baseDmgReduction_Sheild;

    protected Point position;

    public virtual void Tick()
    {

        this.hp += regen_Health;
        this.sheild += regen_Sheild;

        this.hp = Math.Min(this.hp, maxHp);
        this.sheild = Math.Min(this.sheild, maxSheild);
        if (this.hp <= 0)
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

        dmg -= this.dmgReduction_Sheild;
        dmg = Math.Max(dmg, 1);
        this.sheild -= dmg;
        this.dmgReduction_Health = -sheild;

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
        dmg -= this.dmgReduction_Health;
        dmg = Math.Max(dmg, 1);
        this.hp -= dmg;     
    }

    protected abstract void Die();


    
}