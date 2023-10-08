using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

using Size = System.Drawing.Size;
using Bitmap = System.Drawing.Bitmap;

class Grid
{

    private readonly bool[][] isTaken;
    private readonly int[][] enemyValue;    //  how much an enemy value being in a specific tile, equals the reverse distance to player structures

    public Grid(Bitmap sourceImage)
    {
        const int multi = Map.mapPixelToGridTile_Multiplier;
        Size size = sourceImage.Size * multi;

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

    public void Draw()
    {
        //  Draw available tiles
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

}