using System.Collections.Generic;
using Microsoft.Xna.Framework;

class Grid
{

    private bool[][] isTaken;

    public bool IsTileTaken(int x, int y)
    {
        return isTaken[y][x];
    }
    public bool IsTileTaken(Point point)
    {
        return this.IsTileTaken(point.X, point.Y);
    }

    public bool PlaceIfPossible(Building building)
    {
        Rectangle area = building.gridArea;

        for (int x = area.Left; x <= area.Right; x++)
        {
            for (int y = area.Top; y <= area.Bottom; y++)
            {
                if (this.IsTileTaken(x,y))
                {
                    return false;
                }
            }
        }
        //  mark as taken
        for (int x = area.Left; x <= area.Right; x++)
        {
            for (int y = area.Top; y <= area.Bottom; y++)
            {
                isTaken[y][x] = true;
            }
        }
        return true;
    }

    public void RemoveBuilding(Building building)
    {
        Rectangle area = building.gridArea;

        //  mark as taken
        for (int x = area.Left; x <= area.Right; x++)
        {
            for (int y = area.Top; y <= area.Bottom; y++)
            {
                isTaken[y][x] = false;
            }
        } 

    }

    public void Draw()
    {
        //  Draw available tiles
    }

}