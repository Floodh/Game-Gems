using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

abstract class Building : Targetable
{

    public static List<Building> allBuildings = new List<Building>();
    private static Grid grid;

    public static void SetGridSize(Size size)
    {
        grid = new Grid(size);
    }

    public static void DrawAll()
    {
        foreach (Building building in allBuildings)
        {
            building.Draw();
        }
    }

    public bool IsDead{get; private set;} = false;
    public Rectangle GridArea {get; protected set;} = Rectangle.Empty;
    
    //  can overide this
    public Size GridSize{
        get {return new Size(2,2);}
    }
    public Rectangle DrawArea
    {   get
        {   return
            new Rectangle(
                GridArea.X * Map.mapPixelToTexturePixel_Multiplier / Map.mapPixelToGridTile_Multiplier,
                GridArea.Y * Map.mapPixelToTexturePixel_Multiplier / Map.mapPixelToGridTile_Multiplier,
                GridArea.Width * Map.mapPixelToTexturePixel_Multiplier / Map.mapPixelToGridTile_Multiplier,
                GridArea.Height * Map.mapPixelToTexturePixel_Multiplier / Map.mapPixelToGridTile_Multiplier
            );  
        }      
    }

    public bool isSelected = false;

    public virtual void Draw()
    {
        //  if isSelected draw green outline


    }

    public abstract void Update();

    public bool Place(Point position)
    {

        Rectangle area = new Rectangle(position.X, position.Y, this.GridSize.Width, this.GridSize.Height);
        return Place(area);

    }

    private bool Place(Rectangle area)
    {

        if (grid.PlaceIfPossible(this, area))
        {
            allBuildings.Add(this);
            this.GridArea = area;
            return true;
        }
        return false;

    }

    //  returns true if it died
    public override bool Hit(Projectile projectile)
    {
        this.TakeDmg(projectile);
        
        if (this.hp <= 0)
        {
            this.Die();
            return true;
        }

        return false;

    }

    private void Die()
    {
        this.IsDead = true;
        allBuildings.Remove(this);  //  consider delaying the removal of this object from the list for potential death animation
        grid.RemoveBuilding(this);
    }
}

