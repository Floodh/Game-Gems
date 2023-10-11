using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;
using Bitmap = System.Drawing.Bitmap;


abstract class Building : Targetable
{

    public enum Type
    {
        Boulder,
        Cannon,
        Generator,
        Healer,
        Mineral,
        ThePortal,
        Wall
    }

    public static List<Building> allBuildings = new List<Building>();
    public static Grid grid;


    public static void SetGrid(Bitmap sourceImage)
    {
        grid = new Grid(sourceImage);
    }

    public static void DrawAll()
    {
        foreach (Building building in allBuildings)
        {
            building.Draw();
        }
    }

    public static void TickAll()
    {
        for (int i = 0; i < allBuildings.Count; i++)
        {
            Building building = allBuildings[i];
            building.Tick();
            if (building.IsDead)
                i--;
        }
    }

    public override Rectangle GridArea {get; protected set;} = Rectangle.Empty;

    public override Point TargetPosition {
        get 
        {
            return new Point(
                DrawArea.X + DrawArea.Width / 2,
                DrawArea.Y + DrawArea.Height / 2               
            );
        }
    }

    public Building(Faction faction)
        : base(faction)
    {}
    
    //  can overide this
    public static Size GridSize{
        get {return new Size(2,2);}
    }
    public Rectangle DrawArea
    {   get
        {   return
            Grid.ToDrawArea(GridArea);
        }      
    }

    public bool isSelected = false;

    public virtual void Draw()
    {
        //  if isSelected draw green outline
    }

    // public virtual void Action()
    // {}

    public override void Tick()
    {
        base.Tick();
    }

    public bool Place(int x, int y)
    {
        return Place(new Point(x, y));
    }

    public bool Place(Point position)
    {

        Rectangle area = new Rectangle(position.X, position.Y, GridSize.Width, GridSize.Height);
        return Place(area);

    }

    private bool Place(Rectangle area)
    {
        //Console.WriteLine(area);

        if (grid.PlaceIfPossible(this, area))
        {
            allBuildings.Add(this);
            this.GridArea = area;
            return true;
        }
        return false;

    }

    //  returns true if health is negative
    public override bool Hit(Projectile projectile)
    {
        this.TakeDmg(projectile); 
        return this.Hp <= 0;
    }

    protected override void Die()
    {
        this.IsDead = true;
        //Console.WriteLine($"died {}");
        allBuildings.Remove(this);  //  consider delaying the removal of this object from the list for potential death animation
        grid.RemoveBuilding(this);
    }
}

