using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

abstract class Unit : Targetable
{
    protected const int movementRate = 86;

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

    public override Point TargetPosition {
        get 
        {
            return new Point(
                DrawArea.X + DrawArea.Width / 2,
                DrawArea.Y + DrawArea.Height / 2               
            );
        }
    }

    public Unit(Faction faction, Point gridPosition)
        : base(faction)
    {
        allUnits.Add(this);
        this.GridArea = new Rectangle(gridPosition, new Point(1,1));
    }

    protected Rectangle DrawArea 
    {
        get 
        {
            return Grid.ToDrawArea(GridArea);
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
        this.IsDead = true;
        allUnits.Remove(this);
        this.MoveFrom(this.GridArea.Location);
        //  consider spawning death animation
        Console.WriteLine("A unit has died!");
    }

    //  returns true if health is negative
    public override bool Hit(Projectile projectile)
    {
        this.TakeDmg(projectile); 
        return this.Hp <= 0;
    }

    protected bool CanMoveTo(Point gridPosition)
    {
        return !Building.grid.IsTileTaken(gridPosition.X, gridPosition.Y);
    }
    protected void MoveTo(Point gridDestination)
    {
        if (Building.grid.IsTileTaken(gridDestination))
            throw new ArgumentException("Tried to move to a taken tile!");

        Building.grid.Mark(gridDestination, true);
    }
    protected void MoveFrom(Point gridOrigin)
    {
        if (!Building.grid.IsTileTaken(gridOrigin))
            throw new ArgumentException("Tried to move from a position that isn't taken by anything!");
        Building.grid.Mark(gridOrigin, false);
    }
    protected void MoveToFrom(Point gridDestination, Point gridOrigin)
    {
        if (gridDestination == gridOrigin)
            throw new ArgumentException("Tried to move to the same position!");
        MoveFrom(gridOrigin);
        MoveTo(gridDestination);
    }

}
