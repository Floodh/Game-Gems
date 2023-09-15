using System;
using System.Drawing;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Size = System.Drawing.Size;

static class Camera
{
    private static Point offset = Point.Zero;
    private static int zoomDivider = 2;
    private static Size cameraWindowSize;



    public static void Init(Size drawTextureSize)
    {
        Console.WriteLine("inuti camera init");
        cameraWindowSize = new Size(drawTextureSize.Width*5, drawTextureSize.Height*5);
        Console.WriteLine("inuti camera init2");
    }

    //  return a modified  Point
    public static Point PointOffset(Point position)
    {
        position = offset;
        return offset;
    }

    public static Size SizeOffset(Size size)
    { 
        return  new Size(size.Width / zoomDivider, size.Height / zoomDivider);    
    }

    //  return a rectangle modified 
    public static Rectangle rectOffset(Rectangle rectangle)
    {
         Console.WriteLine("inuti rectoffset");
         rectangle = new Rectangle(offset.X,offset.Y,
         cameraWindowSize.Width, cameraWindowSize.Height);
         return rectangle;
    }   

    //
    public static bool IsVisible(Rectangle area)
    {
        return true;
    }

    public static void UpdateByMouse(MouseState mouseState)
    {
       Rectangle temprect = new Rectangle(offset.X, offset.Y, cameraWindowSize.Width, cameraWindowSize.Height);
        // if (mouseState.Position.X ==  )
        // {
        //     offset.X -= 10;
        // }
        if (mouseState.Position.X == temprect.Left )
        {
            offset.X += 10;
        }
        if (mouseState.Position.Y == temprect.Top)
        {
            offset.Y += 10;
        }
        if (mouseState.Position.Y == temprect.Bottom)
        {
            offset.Y -= 10;
        }
    }

    public static void UpdateByKeyboard(KeyboardState keyboardState)
    {
        if(keyboardState.IsKeyDown(Keys.Right))
        {
            offset.X -= 10;
        }
        if(keyboardState.IsKeyDown(Keys.Left))
        {
            offset.X += 10;
        }
        if(keyboardState.IsKeyDown(Keys.Down))
        {
            offset.Y -= 10;
        }
        if(keyboardState.IsKeyDown(Keys.Up))
        {
            offset.Y += 10;
        }
    }
    
}