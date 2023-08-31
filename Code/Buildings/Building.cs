using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

abstract class Building : Targetable
{

    public static List<Building> allBuildings = new List<Building>();

    public static void DrawAll()
    {
        foreach (Building building in allBuildings)
        {
            building.Draw();
        }
    }


    protected Size gridSize;
    protected int hp;

    public void Draw()
    {

    }

    public abstract void Update();

    public void Die()
    {
        allBuildings.Remove(this);
    }
}

