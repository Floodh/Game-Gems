using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

static class TextureSource
{

    public static Texture2D[] LoadStone()
    {

        int xStart = 81;
        int yStart = 74;
        int width = 226 - xStart;
        int height = 219 - yStart;
        int xBuffer = 268 - width - xStart;
        int yBuffer = 259 - height - yStart;

        Point[] selected = new Point[]
        {
                                                                                new Point(4, 0),
                             new Point(1, 1), new Point(2, 1), new Point(3, 1),
                             new Point(1, 2),                  new Point(3, 2), new Point(4, 2),
                             new Point(1, 3),                                                    
                             new Point(1, 4), new Point(2, 4), new Point(3, 4),
        };
        Rectangle[] areas = new Rectangle[selected.Length];
        for (int i = 0; i < selected.Length; i++)
        {
            areas[i] = new Rectangle(
                xStart + (width + xBuffer) * selected[i].X,
                yStart + (height + yBuffer) * selected[i].Y,
                width,
                height
            );
        }

        return Load("Data/TextureSources/Dirt.jpg", areas);

    }


    public static Texture2D[] LoadDirt()
    {

        int xStart = 81;
        int yStart = 74;
        int width = 226 - xStart;
        int height = 219 - yStart;
        int xBuffer = 268 - width - xStart;
        int yBuffer = 259 - height - yStart;

        Point[] selected = new Point[]
        {
            new Point(0, 0), new Point(1, 0), new Point(2, 0),
            new Point(0, 1),
            new Point(0, 2),
                                              new Point(2, 3), new Point(3, 3), new Point(4, 3),
                                                                                new Point(4, 4),
        };
        Rectangle[] areas = new Rectangle[selected.Length];
        for (int i = 0; i < selected.Length; i++)
        {
            areas[i] = new Rectangle(
                xStart + (width + xBuffer) * selected[i].X,
                yStart + (height + yBuffer) * selected[i].Y,
                width,
                height
            );
        }

        return Load("Data/TextureSources/Dirt.jpg", areas);

    }

    public static Texture2D[] LoadGrass()
    {
        int xStart = 49;
        int yStart = 67;
        int width = 153 - xStart;
        int height = 171 - yStart;
        int xBuffer = 224 - width - xStart;
        int yBuffer = 231 - height - yStart;


        Point[] selected = new Point[]
        {
            new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(4, 0),
            new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(3, 1),
            new Point(0, 2), new Point(1, 2),                  new Point(3, 2),
            new Point(0, 3),                                                    new Point(4, 3),
            new Point(0, 4), new Point(1, 4), new Point(2, 4), new Point(3, 4),
            new Point(0, 5),                                   new Point(3, 5), new Point(4, 5),
        };
        Rectangle[] areas = new Rectangle[selected.Length];
        for (int i = 0; i < selected.Length; i++)
        {
            areas[i] = new Rectangle(
                xStart + (width + xBuffer) * selected[i].X,
                yStart + (height + yBuffer) * selected[i].Y,
                width,
                height
            );
        }

        return Load("Data/TextureSources/Grass.jpg", areas);

    }


    public static Texture2D[] LoadWater()
    {

        int xStart = 45;
        int yStart = 53;
        int width = 176 - xStart;
        int height = 184 - yStart;
        int xBuffer = 247 - width - xStart;
        int yBuffer = 251 - height - yStart;

        Point[] selected = new Point[]
        {
            new Point(2, 0), new Point(4, 0),
            new Point(0, 1), new Point(1, 1), new Point(2, 1),
            new Point(0, 2), new Point(1, 2), new Point(2, 2), new Point(3, 2), new Point(4, 2),
            new Point(0, 3), new Point(1, 3), new Point(2, 3), new Point(3, 3), new Point(4, 3),
            new Point(0, 4), new Point(1, 4), new Point(2, 4), new Point(3, 4), new Point(4, 4),
        };
        Rectangle[] areas = new Rectangle[selected.Length];
        for (int i = 0; i < selected.Length; i++)
        {
            areas[i] = new Rectangle(
                xStart + (width + xBuffer) * selected[i].X,
                yStart + (height + yBuffer) * selected[i].Y,
                width,
                height
            );
        }

        return Load("Data/TextureSources/Water.jpg", areas);

    }

    private static Texture2D[] Load(string path, Rectangle[] areas)
    {
        if (areas.Length == 0)
            throw new ArgumentException("Need to supply areas for loading from texture source!");

        int width = areas[0].Width, height = areas[0].Height;

        using Texture2D textureSource = Texture2D.FromFile(GameWindow.graphicsDevice, path);
        using RenderTarget2D renderTargetIsAOffScreenBuffer = new (GameWindow.graphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None);
        GameWindow.graphicsDevice.SetRenderTarget(renderTargetIsAOffScreenBuffer);


        Texture2D[] result = new Texture2D[areas.Length];
        for (int i = 0; i < result.Length; i++)
        {
            GameWindow.graphicsDevice.Clear(Color.Transparent);

            Rectangle copyArea = areas[i];    

            GameWindow.spriteBatch.Begin();
                GameWindow.spriteBatch.Draw(textureSource, new Rectangle(0, 0, width, height), copyArea, Color.White);
            GameWindow.spriteBatch.End();

            using MemoryStream stream = new MemoryStream();
            renderTargetIsAOffScreenBuffer.SaveAsPng(stream, width, height);
            result[i] = Texture2D.FromStream(GameWindow.graphicsDevice, stream); 

            File.WriteAllBytes($"Cache/DirtTest{i}.png", stream.ToArray());
        }

        GameWindow.graphicsDevice.SetRenderTarget(null);
        return result;

    }


}