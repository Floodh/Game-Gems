using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

abstract class Unit : Targetable
{

    const int unitPxSize = 32;

    public static List<Unit> allUnits = new List<Unit>();
    

    public static void DrawAll()
    {
        foreach (Unit unit in allUnits)
        {
            unit.Draw();
        }
    }

    public static void TickAll()
    {
        for (int i = 0; i < allUnits.Count; i++)
        {
            Unit unit = allUnits[i];
            unit.Tick();
            if (unit.IsDead)
                i--;
        }
    }

    public override Point TargetPosition {get {return new Point(((int)exactPosition.X + unitPxSize / 2), ((int)exactPosition.Y + unitPxSize / 2));}}
    protected Vector2 exactPosition;

    public Unit(Faction faction)
        : base(faction)
    {
        allUnits.Add(this);
    }

    protected Rectangle DrawArea {
        get 
        {
            return new Rectangle(((int)exactPosition.X), ((int)exactPosition.Y), unitPxSize, unitPxSize);
        }
    }
    
    public virtual void Draw()
    {}

    public override void Tick()
    {
        base.Tick();
    }

    protected override void Die()
    {

    }

    //  returns true if health is negative
    public override bool Hit(Projectile projectile)
    {
        this.TakeDmg(projectile); 
        return this.Hp <= 0;
    }

    // protected override void Die()
    // {
    //     this.IsDead = true;
    //     //Console.WriteLine($"died {}");
    //     allBuildings.Remove(this);  //  consider delaying the removal of this object from the list for potential death animation
    //     grid.RemoveBuilding(this);
    // }
}
