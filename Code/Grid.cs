using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

using Size = System.Drawing.Size;
using Bitmap = System.Drawing.Bitmap;
using System.Drawing.Imaging;

class Grid
{

    private readonly bool[][] isTaken;
    private readonly int[][] enemyValue;    //  how much an enemy value being in a specific tile, equals the reverse distance to player structures
    private readonly Size size;

    public Grid(Bitmap sourceImage)
    {
        const int multi = Map.mapPixelToGridTile_Multiplier;
        this.size = sourceImage.Size * multi;

        this.isTaken = new bool[size.Height][];
        this.enemyValue = new int[size.Height][];
        for (int y = 0; y < size.Height; y++)
        {
            this.isTaken[y] = new bool[size.Width];
            this.enemyValue[y] = new int[size.Width];
        }

        //  this one could be buggy, if the grid behaves strange along edges, check this
        for (int y = 0; y < size.Height; y++)
        for (int x = 0; x < size.Width; x++)
        {
            if ((Map.TilesRGB)sourceImage.GetPixel(x, y).ToArgb() == Map.TilesRGB.Water)
            {
                Mark(new Rectangle(x * multi, y * multi, multi, multi), true);
                enemyValue[y][x] = int.MinValue;
                //Console.WriteLine($"x={x},y={y} set to {int.MinValue}");
            }
        }
    }

    public bool IsTileTaken(int x, int y)
    {
        return isTaken[y][x];
    }
    public bool IsTileTaken(Point point)
    {
        return this.IsTileTaken(point.X, point.Y);
    }

    public bool PlaceIfPossible(Building building, Point point)
    {
        Size size = Building.GridSize;
        return PlaceIfPossible(building, new Rectangle(point.X, point.Y, size.Width, size.Height));
    }

    public bool PlaceIfPossible(Building building, Rectangle area)
    {
        //Console.WriteLine();
        for (int x = area.Left; x < area.Right; x++)
        {
            for (int y = area.Top; y < area.Bottom; y++)
            {
                if (this.IsTileTaken(x,y))
                {
                    return false;
                }
            }
        }
        //  mark as taken
        this.Mark(area, true);
        return true;
    }

    public bool CanPlace(Rectangle area)
    {

        for (int x = area.Left; x < area.Right; x++)
        {
            for (int y = area.Top; y < area.Bottom; y++)
            {
                if (this.IsTileTaken(x,y))
                {
                    return false;
                }
            }
        } 
        return true; 
              
    }

    public void RemoveBuilding(Building building)
    {
        //  mark as free
        Rectangle area = building.GridArea;
        this.Mark(area, false);
    }

    private void Mark(Rectangle area, bool taken)
    {
        for (int x = area.Left; x < area.Right; x++)
        {
            for (int y = area.Top; y < area.Bottom; y++)
            {
                isTaken[y][x] = taken;
            }
        }         
    }

    public int GetEnemyValue(int x, int y)
    {
        return enemyValue[y][x];
    }

    public void CalculateEnemyValue()
    {
        ClearEnemyValue();
        foreach (Building building in Building.allBuildings)
            if (building.faction == Faction.Player)
        {
            CalculateEnemyValue(building);
        }

        PresentEnemyValue();
    }

    public void CalculateEnemyValue(Building building)
    {
        Point p = building.GridArea.Location;
        for (int dy = 0; dy < building.GridArea.Height; dy++)
        for (int dx = 0; dx < building.GridArea.Width;  dx++)
        {
            int x = p.X + dx,
                y = p.Y + dy;
            CalculateEnemyValue(x, y, 0);
        }
    }

    public static readonly int[] offsets = {1,0,-1,0,0,1,0,-1};
    private void CalculateEnemyValue(int x, int y, int distance)
    {
        for (int i = 0; i < offsets.Length / 2; i++)
        {
            int newX = x + offsets[i * 2];
            int newY = y + offsets[i * 2 + 1];
            int value = int.MaxValue - distance;
            // Console.Write($"newX={newX},newY={newY}");
            // Console.WriteLine($", enemyValue[newY][newX] {enemyValue[newY][newX]}, evaluation = {enemyValue[newY][newX] < value}");
            if (enemyValue[newY][newX] < value)  //  only travel if the value is less, otherwise theres no point
            if (enemyValue[newY][newX] != int.MinValue)
            {
                enemyValue[newY][newX] = value;
                CalculateEnemyValue(newX, newY, distance + 1);
            }
        }
    }

    private void ClearEnemyValue()
    {
        for (int y = 0; y < size.Width; y++)
        for (int x = 0; x < size.Height; x++) 
        {
            if (this.isTaken[y][x])
                enemyValue[y][x] = int.MinValue;
            else
                enemyValue[y][x] = 0;
        }
    }

    private void PresentEnemyValue()
    {
        Bitmap image = new Bitmap(size.Width, size.Height);
        for (int y = 0; y < size.Width; y++)
        for (int x = 0; x < size.Height; x++)
        {
            int colorValue = enemyValue[y][x] - (int.MaxValue - 254);
            if (enemyValue[y][x] == int.MinValue)
                colorValue = 0;
            colorValue = Math.Max(0, colorValue);
            image.SetPixel(x, y, System.Drawing.Color.FromArgb(colorValue, colorValue, colorValue));
        }

        foreach (Building building in Building.allBuildings)
            for (int dy = 0; dy < building.GridArea.Width; dy++)
            for (int dx = 0; dx < building.GridArea.Height; dx++)
        {
            Point p = building.GridArea.Location;
            System.Drawing.Color color = System.Drawing.Color.BurlyWood;
            if (building.faction == Faction.Player)
                color = System.Drawing.Color.Green;
            else if (building.faction == Faction.Enemy)
                color = System.Drawing.Color.Red;

            image.SetPixel(p.X + dx, p.Y + dy, color);            
        }

        image.Save("EnemyValue.png", ImageFormat.Png);

    }

    //  untested
    public static Point WorldToGrid(Point worldPoint)
    {
        return new Point(
            (worldPoint.X * Map.mapPixelToGridTile_Multiplier) / Map.mapPixelToTexturePixel_Multiplier,
            (worldPoint.Y * Map.mapPixelToGridTile_Multiplier) / Map.mapPixelToTexturePixel_Multiplier);
    }


    public static Rectangle ToDrawArea(Rectangle gridArea)
    {
        return new Rectangle(
            gridArea.X * Map.mapPixelToTexturePixel_Multiplier / Map.mapPixelToGridTile_Multiplier,
            gridArea.Y * Map.mapPixelToTexturePixel_Multiplier / Map.mapPixelToGridTile_Multiplier,
            gridArea.Width * Map.mapPixelToTexturePixel_Multiplier / Map.mapPixelToGridTile_Multiplier,
            gridArea.Height * Map.mapPixelToTexturePixel_Multiplier / Map.mapPixelToGridTile_Multiplier
        );  
    }

    public void Draw()
    {
        //  Draw available tiles
    }

}