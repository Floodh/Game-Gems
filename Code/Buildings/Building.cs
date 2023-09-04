using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

abstract class Building : Targetable
{

    public static List<Building> allBuildings = new List<Building>();
    private static Grid grid = new Grid();

    public static void DrawAll()
    {
        foreach (Building building in allBuildings)
        {
            building.Draw();
        }
    }


    protected Size gridSize;
    protected int hp;

    public Rectangle gridArea;

    public void Draw()
    {

    }

    public abstract void Update();

    private bool Place()
    {
        if (grid.PlaceIfPossible(this))
        {
            allBuildings.Add(this);
            return true;
        }
        return false;
    }

    private void Die()
    {
        allBuildings.Remove(this);
        grid.RemoveBuilding(this);
    }
}

