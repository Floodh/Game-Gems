using System.Globalization;
using System;
using System.IO;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//  an object should store its
class EnergyBeam : Animation
{
    public enum Type
    {
        Line,
        Dual
    }

    private static Dictionary<Type, Texture2D[]> textureDict = new();

    public EnergyBeam(Point gridStart, Point gridEnd, Type type)
        : base(RenderFrames(gridStart, gridEnd, type), 8)
    {}

    private static Tuple<Texture2D[], Rectangle> RenderFrames(Point gridStart, Point gridEnd, Type type)
    {

        int dx = gridStart.X - gridEnd.X;
        int dy = gridStart.Y - gridEnd.Y;
        int gcd = GCD(dx, dy);

        //  ratio
        int xStep = dx / gcd;
        int yStep = dy / gcd;

        int xDiff = 0;
        int yDiff = 0;

        int distance = Math.Abs(dx) + Math.Abs(dy);

        List<Rectangle> drawAreas = new();

        Point walkPoint = gridStart;


        for (int i = 0; i < distance; i++)
        {
            

            if (RatioWhichX(xStep, yStep, xDiff, yDiff))
            {
                if (dx < 0)
                    xDiff++;
                else
                    xDiff--;
            }
            else    //  y
            {
                if (dy < 0)
                    yDiff++;
                else
                    yDiff--;
            }

            Point nextPoint = gridStart;
            nextPoint.X += xDiff;
            nextPoint.Y += yDiff;


            Rectangle currentDrawArea = Grid.ToDrawArea(new Rectangle(walkPoint, new Point(2,2)));
            Rectangle nextDrawArea = Grid.ToDrawArea(new Rectangle(nextPoint, new Point(2,2)));
            Rectangle drawArea = new Rectangle(
                (currentDrawArea.Location + nextDrawArea.Location).X / 2, 
                (currentDrawArea.Location + nextDrawArea.Location).Y / 2, 
                nextDrawArea.Size.X, nextDrawArea.Size.Y);

            Console.WriteLine($"walkpoint : {walkPoint} ---> {nextPoint}, draw Area {drawArea}");
            drawAreas.Add(drawArea);

            walkPoint = nextPoint;

        }

        Rectangle encloseingArea = FindEncloseingArea(drawAreas.ToArray());
        Console.WriteLine(encloseingArea);
        Texture2D[] baseTextures = GetBaseTextures(type);
        Texture2D[] textures = RenderTextures(encloseingArea, drawAreas.ToArray(), baseTextures);
        return new Tuple<Texture2D[], Rectangle>(textures, encloseingArea);
    }

    private static Texture2D[] RenderTextures(Rectangle encloseingArea, Rectangle[] areas, Texture2D[] baseTextures)
    {
        int count = 0;
        foreach (Texture2D baseTexture in baseTextures)
        {
            MemoryStream stream = new MemoryStream(); 
            baseTexture.SaveAsPng(stream, baseTexture.Width, baseTexture.Height);
            File.WriteAllBytes($"baseText_{count++}.png", stream.ToArray());            
        }

        Point origo = encloseingArea.Location;
        using RenderTarget2D renderTargetIsAOffScreenBuffer = new (GameWindow.graphicsDevice, encloseingArea.Width, encloseingArea.Height, false, SurfaceFormat.Color, DepthFormat.None);

        GameWindow.spriteBatch.Begin();
        GameWindow.graphicsDevice.SetRenderTarget(renderTargetIsAOffScreenBuffer);

        Texture2D[] result = new Texture2D[baseTextures.Length];
        for (int animationStep = 0; animationStep < baseTextures.Length; animationStep++)
        {
            Texture2D texture = new(GameWindow.graphicsDevice, encloseingArea.Width, encloseingArea.Height);
            GameWindow.graphicsDevice.Clear(Color.Azure);
            for (int i = 0; i < areas.Length; i++)
            {
                
                Rectangle area = areas[i];
                area.Offset(-origo.X, -origo.Y);
                Console.WriteLine($"    {areas[i]} --> {area}");
                int textureIndex = (animationStep + i) % baseTextures.Length;
                GameWindow.spriteBatch.Draw(baseTextures[textureIndex], area, Color.White);
                
            }
            using MemoryStream stream = new MemoryStream();
            renderTargetIsAOffScreenBuffer.SaveAsPng(stream, encloseingArea.Width, encloseingArea.Height);
            texture = Texture2D.FromStream(GameWindow.graphicsDevice, stream);              
            result[animationStep] = texture;

            //  debugg
            File.WriteAllBytes($"prerendered_{animationStep}.png", stream.ToArray());
           
        }

        GameWindow.spriteBatch.End();
        GameWindow.graphicsDevice.SetRenderTarget(null);

        return result;

    }


    private static Texture2D[] GetBaseTextures(Type type)
    {
        if (textureDict.ContainsKey(type))
        {
            return textureDict[type];
        }
        else
        {
            if (type == Type.Line)
            {
                Texture2D[] textures = new Texture2D[4];
                for (int i = 0; i < 4; i++)
                    textures[i] = Texture2D.FromFile(GameWindow.graphicsDevice, $"Data/Texture/Beam_Line_{i}.png");
                textureDict.Add(type, textures);
                return textures;
            }
            else
            {
                throw new ArgumentException($"Invalid Energy Beam type! : {type}");
            }

        }

    }

    private static bool RatioWhichX(int xStep, int yStep, int xDiff, int yDiff)
    {
        //  xStep * yDiff = yStep * xDiff
        //  we wan't these values to be equal
        int need_for_x = xStep * (Math.Abs(yDiff) + 1);
        int need_for_y = yStep * (Math.Abs(xDiff) + 1);
        return need_for_x >= need_for_y;
    }

    private static int GCD(int n1, int n2)
    {
        if (n1 == 0 || n2 == 0)
            return n1 > n2 ? n1 : n2;

        int 
            m1 = n1>0 ? 1 : -1, 
            m2 = n2>0 ? 1 : -1;
            n1 = Math.Abs(n1);
            n2 = Math.Abs(n2);

        while(n1!=n2)
        {
            if(n1 > n2)
                n1 -= n2;
            else
                n2 -= n1;
        }
        return n1 > n2 ? n1 * m1 : n2 * m2;
    }

    public static Rectangle FindEncloseingArea(Rectangle[] areas)
    {
        if (areas.Length == 0)
            return Rectangle.Empty;

        int top = areas[0].Top;
        int bot = areas[0].Bottom;
        int left = areas[0].Left;
        int right = areas[0].Right;

        foreach (Rectangle area in areas)
        {
            if (area.Top < top)
                top = area.Top;
            if (area.Bottom > bot)
                bot = area.Bottom;
            if (area.Left < left)
                left = area.Left;
            if (area.Right > right)
                right = area.Right;
        }

        return new Rectangle(left, top, right - left, bot - top);   //  this is correct btw

    }


}