using System.Globalization;
using System.Collections.Generic;
using System.Data;
using System.Numerics;

class Projectile
{
    public static List<Projectile> allProjectiles = new List<Projectile>();



    public static void TickAll()
    {

        foreach (Projectile projectile in allProjectiles)
        {
            //  this will make a projectile hit instantly
            projectile.target.TakeDmg(projectile);
            projectile.hasHit = true;
        }

        for (int i = 0; i < allProjectiles.Count; i++)
        {
            if (allProjectiles[i].hasHit == true)
            {
                allProjectiles.RemoveAt(i--);
            }
        }

    }

    public static void DrawAll()
    {
    }

    public Projectile(int damage, Targetable target, Targetable sender)
    {
        this.damage = damage;
        this.target = target;
        this.sender = sender;
        allProjectiles.Add(this);
    }

    //Vector2 vector2;
    public int damage = 10;
    //double speed = 3.14;
    Targetable target;
    Targetable sender;
    bool hasHit = false;
    
}