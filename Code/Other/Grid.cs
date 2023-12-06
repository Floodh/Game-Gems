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
    private readonly int[][] playerValue;
    private readonly Size size;


    public Grid(Bitmap sourceImage)
    {
        const int multi = Map.mapPixelToGridTile_Multiplier;
        this.size = sourceImage.Size * multi;

        this.isTaken = new bool[size.Height][];
        this.enemyValue = new int[size.Height][];
        this.playerValue = new int[size.Height][];
        for (int y = 0; y < size.Height; y++)
        {
            this.isTaken[y] = new bool[size.Width];
            this.enemyValue[y] = new int[size.Width];
            this.playerValue[y] = new int[size.Width];
        }

        //  this one could be buggy, if the grid behaves strange along edges, check this
        for (int y = 0; y < size.Height; y++)
        for (int x = 0; x < size.Width; x++)
        {
            if ((Map.TilesRGB)sourceImage.GetPixel(x, y).ToArgb() == Map.TilesRGB.Water)
            {
                Mark(new Rectangle(x * multi, y * multi, multi, multi), true);
                enemyValue[y][x] = int.MinValue;
                this.playerValue[y][x] = int.MinValue;
                //Console.WriteLine($"x={x},y={y} set to {int.MinValue}");
            }
        }
    }

    //  inside does not include the edges, and the Rectangle.Contains uses the <= and >= operators
    public bool InsideBounds(Point gridPoint)
    {
        Rectangle area = new Rectangle(1, 1, this.size.Width - 2, this.size.Height - 2);
        return area.Contains(gridPoint);
    }

    public bool IsTileTaken(int x, int y)
    {
        return isTaken[y][x];
    }
    public bool IsTileTaken(Point point)
    {
        return this.IsTileTaken(point.X, point.Y);
    }

    private void HideUnits()
    {
        foreach (Unit unit in Unit.allUnits)
            this.Mark(unit.GridArea.Location, false);
    }
    private void RevealUnits()
    {
        foreach (Unit unit in Unit.allUnits)
            this.Mark(unit.GridArea.Location, true);
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
        this.CalculateEnemyValue();
    }

    public void Mark(Point gridPosition, bool taken)
    {
        isTaken[gridPosition.Y][gridPosition.X] = taken;
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

    public Targetable FindOcupant(Point gridPoint)
    {

        foreach (Building building in Building.allBuildings)
        {
            if (building.GridArea.Contains(gridPoint))
                return building;
        }
        foreach (Unit unit in Unit.allUnits)
        {
            if (unit.GridArea.Contains(gridPoint))
                return unit;
        }

        return null;

    }

    public int GetEnemyValue(int x, int y)
    {
        return enemyValue[y][x];
    }
    public int GetPlayerValue(int x, int y)
    {
        return playerValue[y][x];
    }

    public void CalculateEnemyValue()
    {

        HideUnits();
        ClearValue(this.enemyValue);
        
        foreach (Building building in Building.allBuildings)
            if (building.faction == Faction.Player && !building.IsDead)
                CalculateEnemyValue(building);
        
        foreach (Unit unit in Unit.allUnits)
            if (unit.faction == Faction.Player && !unit.IsDead)
                CalculateValue(unit.GridArea.X, unit.GridArea.Y, 0, enemyValue);

        RevealUnits();
        //PresentValue(this.enemyValue);
    }

    private void CalculateEnemyValue(Building building)
    {
        Point p = building.GridArea.Location;
        for (int dy = 0; dy < building.GridArea.Height; dy++)
        for (int dx = 0; dx < building.GridArea.Width;  dx++)
        {
            int x = p.X + dx,
                y = p.Y + dy;
            CalculateValue(x, y, 0, this.enemyValue);
        }
    }

    //  same logic as with CalculateEnemyValue(Building building) 
    public void CalculatePlayerValue(Targetable target, Point playerPosition)
    {
        this.Mark(playerPosition, false);
        this.ClearValue(playerValue);
        Rectangle area = target.GridArea;
        for (int x = area.Left; x < area.Right; x++)
            for (int y = area.Top; y < area.Bottom; y++)
            {
                this.CalculateValue(x, y, 0, playerValue);
            }
        PresentValue(this.enemyValue);
        this.Mark(playerPosition, true);
    }

    //  slightly different logic since we wan't to arrive at this point
    public void CalculatePlayerValue(Point gridDestination, Point playerPosition)
    {
        this.Mark(playerPosition, false);
        this.ClearValue(playerValue);
        if (playerValue[gridDestination.Y][gridDestination.X] != int.MinValue)
        {
            playerValue[gridDestination.Y][gridDestination.X] = int.MaxValue;
            this.CalculateValue(gridDestination.X, gridDestination.Y, 1, playerValue);
        }
        else
        {
            Console.WriteLine($"{gridDestination} is taken, value = {playerValue[gridDestination.X][gridDestination.Y]}");
        }
        this.Mark(playerPosition, true);
    }

    public static readonly int[] offsets = {1,0,-1,0,0,1,0,-1};
    private void CalculateValue(int x, int y, int distance, int[][] data)
    {
        for (int i = 0; i < offsets.Length / 2; i++)
        {
            int newX = x + offsets[i * 2];
            int newY = y + offsets[i * 2 + 1];
            int value = int.MaxValue - distance;
            // Console.Write($"newX={newX},newY={newY}");
            // Console.WriteLine($", enemyValue[newY][newX] {enemyValue[newY][newX]}, evaluation = {enemyValue[newY][newX] < value}");
            if (data[newY][newX] < value)  //  only travel if the value is less, otherwise theres no point
            if (data[newY][newX] != int.MinValue)
            {
                data[newY][newX] = value;
                CalculateValue(newX, newY, distance + 1, data);
            }
        }
    }

    private void ClearValue(int[][] data)
    {
        for (int y = 0; y < size.Width; y++)
        for (int x = 0; x < size.Height; x++) 
        {
            if (this.isTaken[y][x])
                data[y][x] = int.MinValue;
            else
                data[y][x] = 0;
        }
    }

    private void PresentValue(int[][] data)
    {
        Bitmap image = new Bitmap(size.Width, size.Height);
        for (int y = 0; y < size.Width; y++)
        for (int x = 0; x < size.Height; x++)
        {
            int colorValue = data[y][x] - (int.MaxValue - 254);
            if (data[y][x] == int.MinValue)
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

        image.Save("Cache/Value.png", ImageFormat.Png);

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